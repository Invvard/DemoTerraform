variable "subscription_id" {
	type	   = string
	validation {
		condition	  = can(regex("(?i)[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}", var.subscription_id))
		error_message = "The subscription_id variable must be a valid GUID."
	}
}
variable "client_id" {
	type	   = string
	validation {
		condition	  = can(regex("(?i)[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}", var.client_id))
		error_message = "The client_id variable must be a valid GUID."
	}
}
variable "client_secret" {
	type = string
	sensitive = true
}
variable "tenant_id" {
	type	   = string
	validation {
		condition	  = can(regex("(?i)[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}", var.tenant_id))
		error_message = "The tenant_id variable must be a valid GUID."
	}
}

variable "environment_name" {}
variable "location" { default = "eastus2" }

locals {
  project_name = "demotf"
  resource_prefix = "${local.project_name}-${var.environment_name}"

  tags = {
    project     = local.project_name,
    environment = "default_should_be_set"
  }
}
