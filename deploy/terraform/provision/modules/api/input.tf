variable "location" {}
variable "tags" {}

variable "project_name" {}
variable "environment_name" {}

locals {
  resource_prefix = "${var.project_name}-${var.environment_name}"

  api_name = "dowepush"
  storage_account_name  = "sa${var.project_name}${var.environment_name}${local.api_name}"
  app_service_plan_name = "asp-${local.resource_prefix}-${local.api_name}"
  function_name         = "fct-${local.resource_prefix}-${local.api_name}"

  storage_tier             = "Standard"
  storage_replication_type = "LRS"
  app_service_plan_kind    = "FunctionApp"
  sku_tier                 = "Dynamic"
  sku_size                 = "Y1"
}
