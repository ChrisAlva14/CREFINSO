# Sistema de Control de Créditos y Pagos para la Empresa CREFINSO

## Descripción del Proyecto

Este proyecto consiste en el desarrollo e implementación de un sistema web para la **Cooperativa de Ahorro y Crédito Crecimiento Financiero de Sonsonate (CREFINSO de RL)**. El sistema tiene como objetivo facilitar la gestión de clientes que han adquirido créditos y automatizar la emisión de recibos, eliminando los procesos manuales, mejorando la eficiencia y reduciendo los errores.

El sistema permitirá:
- Registrar, actualizar, eliminar y consultar información de clientes de manera ágil y centralizada.
- Automatizar la emisión de recibos para mejorar la atención al cliente.
- Ajustar los pagos mensuales en función de los abonos realizados (más o menos de la cuota).

## Características Principales

- **Gestión de clientes**: Alta, baja y modificación de clientes.
- **Gestión de créditos**: Registro de créditos, plazos, cuotas mensuales y saldo pendiente.
- **Gestión de pagos**: Registro de pagos con ajustes dinámicos según abonos mayores o menores a la cuota mensual.
- **Ajuste automático de cuotas**: Si un cliente abona más o menos de su cuota, el sistema ajustará el saldo pendiente y modificará las cuotas futuras.
- **Generación automática de recibos**: Los recibos se generan automáticamente tras cada pago.

## Requisitos

### Requisitos Técnicos

- **Lenguaje de Programación**: C#
- **Framework**: ASP.NET Core
- **Base de Datos**: SQL Server
- **ORM**: Entity Framework Core
- **Frontend**: Razor Pages, Tailwind

### Requisitos de Hardware

- Computadora o servidor para el hosting del sistema.
- Impresora térmica para emitir recibos.

## Estructura del Proyecto

### Entidades

- **Cliente**: Información del cliente, como nombre, dirección, teléfono, y email.
- **Crédito**: Información del crédito otorgado al cliente, como monto total, cuotas mensuales, plazo en meses, y saldo pendiente.
- **Pago**: Información sobre los pagos realizados por los clientes, incluyendo el monto abonado, saldo restante y cuota ajustada.

### Base de Datos

El sistema utiliza **SQL Server** como gestor de base de datos. Las tablas principales incluyen:

- `Clientes`: Almacena la información básica de los clientes.
- `Creditos`: Registra los créditos otorgados a los clientes, con los plazos, cuotas mensuales, y saldo pendiente.
- `Pagos`: Mantiene un registro detallado de los pagos realizados y ajusta los montos pendientes y las cuotas mensuales.
  
  
  ## Autores
  1. **Nombre**: Christopher Emmanuel Villalta Alvarenga
  	- **Rol:** Software Developer
  2. **Nombre**: Adilson Ariel Lainez González
  	- **Rol:** Software Developer

