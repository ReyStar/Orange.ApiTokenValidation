INSERT INTO public.tokentable(issuer, audience, private_key, ttl, expiration_time, is_active, payload)
VALUES(@issuer, @audience, @private_key, @ttl, @expiration_time, @is_active, @payload)
RETURNING issuer, audience;