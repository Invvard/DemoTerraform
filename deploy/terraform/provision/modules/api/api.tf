resource "azurerm_storage_account" "storage" {
  name                     = var.storage_account_name
  account_tier             = var.storage_tier
  account_replication_type = var.storage_replication_type

  # General info
  location            = var.location
  resource_group_name = var.resource_group_name
  tags                = var.tags
}

resource "azurerm_app_service_plan" "app_service_plan" {
  name = var.app_service_plan_name
  kind = var.app_service_plan_kind

  sku {
    tier = var.asp_sku_tier
    size = var.asp_sku_size
  }

  # General info
  location            = var.location
  resource_group_name = var.resource_group_name
  tags                = var.tags
}

resource "azurerm_function_app" "function" {
  name       = var.function_name
  version    = "~3"
  https_only = true

  app_settings = {
    WEBSITE_RUN_FROM_PACKAGE                   = 1
    FUNCTIONS_WORKER_RUNTIME                   = "dotnet"
    FUNCTIONS_EXTENSION_VERSION                = "~3"
    "RepositoryOptions:CosmosConnectionString" = var.cosmosDbConnectionString
    "RepositoryOptions:DatabaseId"             = var.cosmosDbDatabaseName
    "RepositoryOptions:ContainerId"            = var.cosmosDbContainerName
    "OidcApiAuthorizationSettings:IssuerUrl"   = var.auth0_url
    "OidcApiAuthorizationSettings:Audience"    = var.auth0_domain
  }

  # Parent resource info
  app_service_plan_id        = azurerm_app_service_plan.app_service_plan.id
  storage_account_name       = azurerm_storage_account.storage.name
  storage_account_access_key = azurerm_storage_account.storage.primary_access_key

  # General info
  location            = var.location
  resource_group_name = var.resource_group_name
  tags                = var.tags
}
