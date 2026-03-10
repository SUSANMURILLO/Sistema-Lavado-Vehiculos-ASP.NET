CREATE DATABASE Proyecto3DB;
GO

USE LavadoVehiculosDB;
GO

-- Tabla Clientes
CREATE TABLE Cliente (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Identificacion VARCHAR(12) NOT NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    Activo BIT NOT NULL DEFAULT 1
);
GO

-- Tabla Empleados
CREATE TABLE Empleados (
    Cedula VARCHAR(12) PRIMARY KEY,
    FechaNacimiento DATE NOT NULL,
    FechaIngreso DATE NOT NULL,
    SalarioXDia FLOAT NOT NULL,
    VacacionesAcumuladas INT NOT NULL,
    FechaDeRetiro DATE NULL
);
GO

-- Tabla Vehículos
CREATE TABLE RegistroVehiculos (
    Placa VARCHAR(10) PRIMARY KEY,
    Marca NVARCHAR(50) NOT NULL,
    Modelo NVARCHAR(50) NOT NULL,
    Tracion NVARCHAR(20),
    Color NVARCHAR(30),
    UltimaFechaDeAtencion DATE NOT NULL,
    TratamientoEspecial BIT NOT NULL,
    ClienteId INT NOT NULL,
    FOREIGN KEY (ClienteId) REFERENCES Cliente(Id)
);
GO

-- Tabla Lavado de Vehículos
CREATE TABLE Lavado (
    IdLavado INT IDENTITY(1,1) PRIMARY KEY,
    PlacaVehiculo VARCHAR(10) NOT NULL,
    IdCliente INT NOT NULL,
    IdEmpleado VARCHAR(12) NOT NULL,
    TiposDeLavado NVARCHAR(50) NOT NULL,
    Pago DECIMAL(10,2) NOT NULL,
    Impuesto DECIMAL(10,2) NOT NULL,
    Total AS (Pago + Impuesto) PERSISTED,
    Estado NVARCHAR(20) NOT NULL,
    FOREIGN KEY (PlacaVehiculo) REFERENCES RegistroVehiculos(Placa),
    FOREIGN KEY (IdCliente) REFERENCES Cliente(Id),
    FOREIGN KEY (IdEmpleado) REFERENCES Empleados(Cedula)
);
GO
