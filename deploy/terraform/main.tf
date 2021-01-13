terraform {
  backend "azurerm" {
	resource_group_name		= var.resource_group_name
    storage_account_name 	= var.storage_account_name
    container_name       	= var.container_name
    key                  	= var.key
    use_msi					= true
  }
}

provider "azurerm" {
	version = "2.4"

	subscription_id	= var.arm_subscription_id
	client_id       = var.arm_client_id
	client_secret   = var.arm_client_secret
	tenant_id       = var.arm_tenant_id

	features {}
}

resource "azurerm_resource_group" "terraforming" {
	name		= var.resourceGroupName
	location	= var.location
	
	tags = var.tags
}

resource "azurerm_storage_account" "terraforming" {
	name						= var.storageAccountName
	resource_group_name			= azurerm_resource_group.terraforming.name
	location					= azurerm_resource_group.terraforming.location
	account_tier				= var.sa_account_tier
	account_replication_type	= var.sa_account_replication_type
	
	tags						= azurerm_resource_group.terraforming.tags
}

resource "azurerm_app_service_plan" "terraforming" {
	name                = "app-service-plan-terraforming"
	location            = azurerm_resource_group.terraforming.location
	resource_group_name = azurerm_resource_group.terraforming.name
	kind				= var.app_service_plan_kind

	sku {
		tier = var.sku_tier
		size = var.sku_size
	}

	tags	= azurerm_resource_group.terraforming.tags
}

resource "azurerm_function_app" "terraforming" {
	name                      = var.functionName
	location                  = azurerm_resource_group.terraforming.location
	resource_group_name       = azurerm_resource_group.terraforming.name
	app_service_plan_id       = azurerm_app_service_plan.terraforming.id
	storage_connection_string = azurerm_storage_account.terraforming.primary_connection_string
	version					  = "~2"

    app_settings = {
        https_only					= true
        WEBSITE_RUN_FROM_PACKAGE	= 1
		FUNCTIONS_WORKER_RUNTIME	= "dotnet"
		FUNCTIONS_EXTENSION_VERSION	= "~3"
    }

	tags	= azurerm_resource_group.terraforming.tags
}
