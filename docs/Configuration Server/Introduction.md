## Overview

Introduction message here, overview of the solution.

## Problem

There are several solutions available for loading application configuration from a source other than disk. Many cloud platforms centralize their configuration through the cloud platform, and provide a way to load those configuration values into the web application host environment variables. 

These solutions work fine but come either with a financial expense or limit developers to dependencies to a specific cloud platform. 

## Solution

There are a few options that allow services to pull configuration.  Shared storage hosting configuration files accessed from services. These mechanisms still rely on cloud platforms or don't provide appropriate security mechanisms to ensure allowed access, nor do they provide mechanisms for dynamic configuration changes.

A cloud-agnostic low-latency centralized configuration system abstracts away these cloud specific configuration systems, allows for services to access a simple service for hosting configurations, including pub/sub mechanisms for automat,ically reloading configuration changes. 

This type of system allows dependent services to utilize a single source of configuration. These systems are important in cloud-scale systems where new instances of a service may require access to a consistent configuration when reading from disk or environment variables are not an option. 

Configuration as a Service (CaaS) systems like this one provide a mechanism for pushing mass configuration updates to subscribed services at the time of configuration change utilizing an efficient configuration protocol.

## Guides

Reference other information in documentation here.

## Source Code

Link to source code Git projects and related.

## Additional Help

Reference links to any additional content related to the solution.