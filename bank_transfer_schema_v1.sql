-- Crear tabla de cuentas
CREATE TABLE accounts (
    id SERIAL PRIMARY KEY,
    account_number VARCHAR(20) UNIQUE NOT NULL,
    account_holder VARCHAR(100) NOT NULL,
    balance DECIMAL(15, 2) NOT NULL DEFAULT 0.00,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);
-- Crear tabla de transferencias
CREATE TABLE transfers (
    id SERIAL PRIMARY KEY,
    source_account_id INT NOT NULL,
    destination_account_id INT NOT NULL,
    amount DECIMAL(15, 2) NOT NULL,
    transfer_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_source_account
        FOREIGN KEY (source_account_id) 
        REFERENCES accounts(id)
        ON DELETE CASCADE,

    CONSTRAINT fk_destination_account
        FOREIGN KEY (destination_account_id)
        REFERENCES accounts(id)
        ON DELETE CASCADE
);
-- Crear tabla de usuario(JWT)
CREATE TABLE Users (
    Id SERIAL PRIMARY KEY,
    Username VARCHAR(50) NOT NULL,
    PasswordHash VARCHAR(256) NOT NULL
);