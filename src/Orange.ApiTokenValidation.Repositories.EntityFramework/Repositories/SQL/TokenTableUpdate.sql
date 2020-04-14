-- DO merge if row already exist
-- https://stackoverflow.com/questions/36799104/how-to-correctly-do-upsert-in-postgres-9-5
INSERT INTO tokentable (issuer, audience, private_key, ttl, expiration_time, is_active, payload, created_time, creator, updated_time, updater)
VALUES (@issuer, @audience, @private_key, @ttl, @expiration_time, @is_active, @payload, @created_time, @creator, @updated_time, @updater)
ON CONFLICT (issuer, audience)
DO UPDATE
SET private_key = @private_key, ttl = @ttl, expiration_time = @expiration_time, is_active = @is_active, payload = @payload ;