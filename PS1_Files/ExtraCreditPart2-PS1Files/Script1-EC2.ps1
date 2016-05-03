$subscr="Michael O'Keefe Azure Pass"
$staccount="mgokstorage"
Select-AzureSubscription -SubscriptionName $subscr –Current
Set-AzureSubscription -SubscriptionName $subscr -CurrentStorageAccountName $staccount
