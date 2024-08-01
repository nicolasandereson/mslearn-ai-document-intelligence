using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;

// Store connection information
string endpoint = "https://prebuiltdocumentintelligence.cognitiveservices.azure.com/";
string apiKey = "6403912e07664538bbc4516165395bda";

Uri fileUri = new Uri("https://github.com/MicrosoftLearning/mslearn-ai-document-intelligence/blob/main/Labfiles/01-prebuild-models/sample-invoice/sample-invoice.pdf?raw=true");

Console.WriteLine("\nConnecting to Forms Recognizer at: {0}", endpoint);
Console.WriteLine("Analyzing invoice at: {0}\n", fileUri.ToString());

// Create the client
var cred = new AzureKeyCredential(apiKey);
var client = new DocumentAnalysisClient(new Uri(endpoint), cred);

// Analyze the invoice
AnalyzeDocumentOperation operation = await client.AnalyzeDocumentFromUriAsync(WaitUntil.Completed, "prebuilt-invoice", fileUri);

// Display invoice information to the user
AnalyzeResult result = operation.Value;




// Iterate through each analyzed document in the result set
foreach (AnalyzedDocument invoice in result.Documents)
{
    // Attempt to retrieve the "VendorName" field from the document
    if (invoice.Fields.TryGetValue("VendorName", out DocumentField? vendorNameField))
    {
        // Check if the field type is a string
        if (vendorNameField.FieldType == DocumentFieldType.String)
        {
            // Extract the vendor name and print it with confidence level
            string vendorName = vendorNameField.Value.AsString();
            Console.WriteLine($"Vendor Name: '{vendorName}', with confidence {vendorNameField.Confidence}.");
        }
    }

    // Attempt to retrieve the "CustomerName" field from the document
    if (invoice.Fields.TryGetValue("CustomerName", out DocumentField? customerNameField))
    {
        // Check if the field type is a string
        if (customerNameField.FieldType == DocumentFieldType.String)
        {
            // Extract the customer name and print it with confidence level
            string customerName = customerNameField.Value.AsString();
            Console.WriteLine($"Customer Name: '{customerName}', with confidence {customerNameField.Confidence}.");
        }
    }

    // Attempt to retrieve the "InvoiceTotal" field from the document
    if (invoice.Fields.TryGetValue("InvoiceTotal", out DocumentField? invoiceTotalField))
    {
        // Check if the field type is a currency
        if (invoiceTotalField.FieldType == DocumentFieldType.Currency)
        {
            // Extract the invoice total and print it with confidence level
            CurrencyValue invoiceTotal = invoiceTotalField.Value.AsCurrency();
            Console.WriteLine($"Invoice Total: '{invoiceTotal.Symbol}{invoiceTotal.Amount}', with confidence {invoiceTotalField.Confidence}.");
        }
    }
}
Console.WriteLine("\nAnalysis complete.\n");