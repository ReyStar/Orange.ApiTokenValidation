SELECT pg_terminate_backend(pid) 
FROM pg_stat_get_activity(NULL::integer) 
WHERE datid=(SELECT oid from pg_database where datname = '{0}');

DROP DATABASE "{0}" ;