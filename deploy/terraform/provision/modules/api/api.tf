resource "azurerm_resource_group" "api_resource_group" {
  name     = "rg-${local.resource_prefix}"
  location = var.location
  tags     = var.tags
}

resource "azurerm_storage_account" "storage" {
  name                     = local.storage_account_name
  account_tier             = local.storage_tier
  account_replication_type = local.storage_replication_type

  # General info
  location            = var.location
  resource_group_name = azurerm_resource_group.api_resource_group.name
  tags                = var.tags
}

resource "azurerm_app_service_plan" "app_service_plan" {
  name = local.app_service_plan_name
  kind = local.app_service_plan_kind

  sku {
    tier = local.sku_tier
    size = local.sku_size
  }

  # General info
  location            = var.location
  resource_group_name = azurerm_resource_group.api_resource_group.name
  tags                = var.tags
}

resource "azurerm_function_app" "function" {
  name       = local.function_name
  version    = "~3"
  https_only = true

  app_settings = {
    WEBSITE_RUN_FROM_PACKAGE                   = 1
    FUNCTIONS_WORKER_RUNTIME                   = "dotnet"
    FUNCTIONS_EXTENSION_VERSION                = "~3"
  }

  # Parent resource info
  app_service_plan_id        = azurerm_app_service_plan.app_service_plan.id
  storage_account_name       = azurerm_storage_account.storage.name
  storage_account_access_key = azurerm_storage_account.storage.primary_access_key

  # General info
  location            = var.location
  resource_group_name = azurerm_resource_group.api_resource_group.name
  tags                = var.tags
}
