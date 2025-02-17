using System;
using System.Collections.Generic;
using Pulumi;
using AzureNative = Pulumi.AzureNative;
using SyncedFolder = Pulumi.SyncedFolder;

return await Deployment.RunAsync(() =>
{
    // Import the program's configuration settings.
    var config = new Config();
    var path = config.Get("path") ?? "./www";
    var indexDocument = config.Get("indexDocument") ?? "index.html";
    var errorDocument = config.Get("errorDocument") ?? "error.html";

   
    // Create a resource group for the website.
    // add tags to resource group

    var resourceGroup = new AzureNative.Resources.ResourceGroup("resource-group", new()
    {
        ResourceGroupName = "website-rg",
        Tags = {
            { "environment", "dev" },
            { "project", "website" },
        },
         
    });
    

    // Create a blob storage account.
    var account = new AzureNative.Storage.StorageAccount("account", new()
    {
        ResourceGroupName = resourceGroup.Name,
        Kind = "StorageV2",
        Sku = new AzureNative.Storage.Inputs.SkuArgs
        {
            Name = "Standard_LRS",
        },
    });

    // Configure the storage account as a website.
    var website = new AzureNative.Storage.StorageAccountStaticWebsite("website", new()
    {
        ResourceGroupName = resourceGroup.Name,
        AccountName = account.Name,
        IndexDocument = indexDocument,
        Error404Document = errorDocument,
    });

    // Use a synced folder to manage the files of the website.
    var syncedFolder = new SyncedFolder.AzureBlobFolder("synced-folder", new()
    {
        Path = path,
        ResourceGroupName = resourceGroup.Name,
        StorageAccountName = account.Name,
        ContainerName = website.ContainerName,
    });

    
    // Export the URLs and hostnames of the storage account
    return new Dictionary<string, object?>
    {
        ["originURL"] = account.PrimaryEndpoints.Apply(primaryEndpoints => primaryEndpoints.Web)
        
    };
});
