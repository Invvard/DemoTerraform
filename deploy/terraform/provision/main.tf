module "api_functions" {
  source = "./modules/api"

  project_name     = local.project_name
  environment_name = var.environment_name

  location = var.location
  tags     = merge(local.tags, { environment = var.environment_name })
}