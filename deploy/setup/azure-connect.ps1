function Get-AzureCLI-Install {
    # paths: x86 and x64 registry keys are different
    if ([IntPtr]::Size -eq 4) {
        $path = 'HKLM:\Software\Microsoft\Windows\CurrentVersion\Uninstall\*'
    }
    else {
        $path = @(
            'HKLM:\Software\Microsoft\Windows\CurrentVersion\Uninstall\*'
            'HKLM:\Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\*'
        )
    }

    # get all data
    $azurecli_count = @(Get-ItemProperty $path |
        # use only with name and uninstall information
        . { process { if ($_.DisplayName -and $_.UninstallString) { $_ } } } |
        # select more or less common subset of properties
        Select-Object DisplayName |
        Where-Object { $_.DisplayName -match 'Microsoft Azure CLI' }).Count

    return $azurecli_count -ge 1
}

function Get-UserAction {
    do {
        Clear-Host
        Write-Output "What to do ?"
        Write-Output "  1. Get RBAC credentials"
        Write-Output ""
        Write-Output "  0. Quit"
        Write-Output ""
        $user_choice = Read-Host -Prompt "Your choice"
    
        Clear-Host

        switch ($user_choice) {
            1 { Get-RbacCreds }
            Default { }
        }
    } while ($user_choice -ne 0)

    Clear-Host
}

function Get-RbacCreds {

    # AZ CMD #1 : Get Azure Account list
    $azAccounts = (az account list | ConvertFrom-Json)
    Clear-Host
    
    Write-Output "Azure Account list :"
    for ($i = 0; $i -lt $azAccounts.Count; $i++) {
        Write-Output "  $i. $($azAccounts[$i].name) ($($azAccounts[$i].id))"
        
    }
    $chosenAccountNum = Read-Host -Prompt "Choose one"
    Clear-Host

    $AccountId = $azAccounts[$chosenAccountNum].id
    $DateTime = Get-Date -Format "yyyy-MM-dd-HH-mm-ss"

    # AZ CMD #2 : Set the current subscription
    az account set --subscription=$AccountId

    # AZ CMD #3 : Create a contributor for RBAC
    $rbacs = (az ad sp create-for-rbac --role="Contributor" --scopes="/subscriptions/$AccountId" | ConvertFrom-Json)
    Clear-Host

    $ClientId = $rbacs.appId
    $ClientSecret = $rbacs.password
    $TenantId = $rbacs.tenant

    Write-Output "arm_subscription_id   = `"$AccountId`""
    Write-Output "arm_client_id         = `"$ClientId`""
    Write-Output "arm_client_secret     = `"$ClientSecret`""
    Write-Output "arm_tenant_id         = `"$TenantId`""
    Write-Output ""

    Read-Host -Prompt "Press enter to continue"
    Clear-Host
}

Clear-Host

if (Get-AzureCLI-Install) {
    Write-Output "Azure CLI is already installed : skipped"
}
else {
    Write-Output "Azure CLI is missing : Installing..."

    Invoke-WebRequest -Uri https://aka.ms/installazurecliwindows -OutFile .\AzureCLI.msi
    Start-Process msiexec.exe -Wait -ArgumentList '/I AzureCLI.msi /quiet'
    Remove-Item .\AzureCLI.msi
}

$groupOutput = az group list
if (!$groupOutput){
    Clear-Host
    Write-Output "Authenticating into your Azure Account"
    az login
}

Get-UserAction