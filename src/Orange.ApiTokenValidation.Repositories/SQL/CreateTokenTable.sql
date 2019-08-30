CREATE TABLE public.tokentable
(
    issuer character varying(255) NOT NULL,
    audience character varying(255) NOT NULL,
    private_key character varying(255) NOT NULL,
    ttl integer NOT NULL,
    expiration_time timestamp with time zone,
    is_active boolean NOT NULL,
    payload jsonb,
    created_time time with time zone,
    creater character varying(50),
    update_time time with time zone,
    updater character varying(50),
    is_deleted boolean,
    CONSTRAINT tokentable_pk PRIMARY KEY (issuer, audience)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.tokentable
    OWNER to sqluser;