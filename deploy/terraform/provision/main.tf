resource "azurerm_resource_group" "api_resource_group" {
  name     = "rg-${local.resource_prefix}"
  location = var.location
  
  tags     = merge(local.tags, {
    environment = var.environment_name
  })
}

module "azfunc_dowepush" {
  source = "./modules/api"

  project_name     = local.project_name
  environment_name = var.environment_name
  resource_prefix  = local.resource_prefix
  rg_name          = azurerm_resource_group.api_resource_group.name
  api_name         = "dowepush"

  location = var.location
  tags     = merge(local.tags, {
    environment = var.environment_name
  })
}

module "azfunc_maybe" {
  source = "./modules/api"

  project_name     = local.project_name
  environment_name = var.environment_name
  resource_prefix  = local.resource_prefix
  rg_name          = azurerm_resource_group.api_resource_group.name
  api_name         = "maybe"

  location = var.location
  tags     = merge(local.tags, {
    environment = var.environment_name
  })
}