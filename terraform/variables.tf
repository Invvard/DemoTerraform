variable "arm_subscription_id" {}
variable "arm_client_id" {}
variable "arm_client_secret" {}
variable "arm_tenant_id" {}
variable "resourceGroupName" {}
variable "storageAccountName" {}
variable "functionName" {}

variable "location" {
  default = "eastus"
}

variable "sa_account_tier" {
  default = "Standard"
}

variable "app_service_plan_kind" {
  default = "FunctionApp"
}

variable "sku_tier" {
  default = "Dynamic"
}

variable "sku_size" {
  default = "Y1"
}

variable "sa_account_replication_type" {
  default = "LRS"
}

variable "tags" {
  type = map
  default = {
    environment = "Terraforming from scratch"
  }
}
