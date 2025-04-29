ALTER TABLE {table_name}
ADD COLUMN is_deleted boolean default false not null,
ADD COLUMN create_time timestamp with time zone default CURRENT_TIMESTAMP not null,
ADD COLUMN update_time timestamp with time zone default CURRENT_TIMESTAMP not null,
ADD COLUMN create_user_id uuid default '00000000-0000-0000-0000-000000000000'  not null,
ADD COLUMN update_user_id uuid default '00000000-0000-0000-0000-000000000000' not null ;