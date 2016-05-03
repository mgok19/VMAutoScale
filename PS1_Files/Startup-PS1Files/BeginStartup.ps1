$vms = Get-AzureVM -ServiceName "mgokvm1"
foreach($vm in $vms)
{
    Set-AzureVMCustomScriptExtension -VM $vm -StorageAccountName "mgokstorage" -StorageAccountKey "7Otr5u7yvQDWDWBs9wDc87nrIw+ztIeUROrgCN9PFt4BJpAfsL68lV6+8XZvFVM71syODre4UeTDXxnTK9WiXA==" –ContainerName "mgok-myscripts" –FileName "startup.ps1"
    $vm | Update-AzureVM
}