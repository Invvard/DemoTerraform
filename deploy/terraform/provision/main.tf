module "azfunc_dowepush" {
  source = "./modules/api"

  project_name     = local.project_name
  environment_name = var.environment_name
  api_name         = "dowepush"

  location = var.location
  tags     = merge(local.tags, {
    environment = var.environment_name
  })
}