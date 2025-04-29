const scriptName = "15_update_Client_AddCountryCurrency";
const version = NumberInt(1);

if (db.MigrationHistory.findOne({ ScriptName: scriptName, Version: version })) {
    print(`${scriptName} v${version} is already applied`);
    quit();
}

const countries = db.Country.find().toArray();
const defaultCountryId = UUID("a8e9b8b2-d3f1-44a1-a149-0fbaabf7e81d");
const defaultCountryName = "International";
const defaultCurrencyId = UUID("04d123f0-dc7e-4b92-829c-dffd1ef0b89a");
const defaultCurrencyCode = "EUR";

const countryMap = {
    US: { country: "USA" },
    GE: { country: "Georgia" },
    CZ: { country: "Czech" },
    PL: { country: "Poland" },
    BY: { country: "Belarus" },
    IT: { country: "Italy" },
    AU: { country: "Australia" },
    CA: { country: "Canada" },
    FR: { country: "France" },
    BG: { country: "Bulgaria" },
    GB: { country: "United Kingdom" },
    DE: { country: "Germany" }
};

function getCountryData(bankAccountNumber) {
    if (!bankAccountNumber) {
        return null;
    }

    const countryCode = bankAccountNumber.substring(0, 2).toUpperCase();
    const countryInfo = countryMap[countryCode];
    if (!countryInfo) return null;

    return countries.find(x => x.Name === countryInfo.country);
}

db.Client.find().forEach(client => {
    const countryData = getCountryData(client.BankAccountNumber);

    const update = {
        CountryId: defaultCountryId,
        CountryName: defaultCountryName,
        CurrencyId: defaultCurrencyId,
        CurrencyCode: defaultCurrencyCode
    };

    if (countryData) {
        update.CountryId = countryData._id;
        update.CountryName = countryData.Name;
        update.CurrencyId = countryData.CurrencyId;
        update.CurrencyCode = countryData.CurrencyCode;

    }

    db.Client.updateOne({ _id: client._id }, { $set: update });
});

db.MigrationHistory.insertOne({
    ScriptName: scriptName,
    Version: version,
    ScriptRunTime: new Date()
});

print("Clients updated with country and currency based on bank account number.");
