  UPDATE public.tokentable
  SET issuer=@issuer, audience=@audience, private_key = @private_key, ttl=@ttl, expiration_time=@expiration_time, is_active=@is_active, payload=@payload
  WHERE issuer = @issuerKey AND audience = @audienceKey