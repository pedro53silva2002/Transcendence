ALTER TABLE auth.users
    ALTER COLUMN oauth_provider DROP DEFAULT;

ALTER TABLE auth.users
    ALTER COLUMN oauth_provider TYPE TEXT
    USING oauth_provider::text;

ALTER TABLE auth.users
    ALTER COLUMN oauth_provider SET DEFAULT 'none';

DROP TYPE IF EXISTS provider_type;

