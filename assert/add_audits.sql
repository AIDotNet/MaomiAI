ALTER TABLE {table_name}
ADD COLUMN is_deleted boolean default false not null,
ADD COLUMN create_time timestamp with time zone default CURRENT_TIMESTAMP not null,
ADD COLUMN update_time timestamp with time zone default CURRENT_TIMESTAMP not null,
ADD COLUMN create_user_id uuid default '00000000-0000-0000-0000-000000000000'  not null,
ADD COLUMN update_user_id uuid default '00000000-0000-0000-0000-000000000000' not null ;

comment on column {table_name}.is_deleted is '软删除';
comment on column {table_name}.create_time is '创建时间';
comment on column {table_name}.update_time is '更新时间';
comment on column {table_name}.create_user_id is '创建人';
comment on column {table_name}.update_user_id is '更新人';
