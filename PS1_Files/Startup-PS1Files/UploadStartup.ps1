$context = New-AzureStorageContext -StorageAccountName "mgokstorage" -StorageAccountKey "7Otr5u7yvQDWDWBs9wDc87nrIw+ztIeUROrgCN9PFt4BJpAfsL68lV6+8XZvFVM71syODre4UeTDXxnTK9WiXA=="
Set-AzureStorageBlobContent -Blob "startup.ps1" -Container "mgok-myscripts" -File "c:\Users\Michael\Desktop\startup.ps1" -Context $context -Force