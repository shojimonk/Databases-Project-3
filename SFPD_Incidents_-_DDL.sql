
drop database if exists "LU.ENGI3675.Proj03";

create database "LU.ENGI3675.Proj03";

drop role if exists "Shoji";
create role "Shoji" login superuser;
comment on role "Shoji" is 'Personal developer superuser';

drop role if exists "Iago Rios";
create role "Shoji" login superuser;
comment on role "Shoji" is 'Personal developer superuser';

drop role if exists "LU.ENGI3675";
create role "LU.ENGI3675" login;
comment on role "LU.ENGI3675" is 'Restricted IIS app pool user';

grant connect on database "LU.ENGI3675.Proj03" to "LU.ENGI3675";  --still doesn't allow any read/write. only connection.
grant connect on database "LU.ENGI3675.Proj03" to "DefaultAppPool";

\connect LU.ENGI3675.Proj03

drop table if exists Crime_Reports cascade;

create table Crime_Reports(
	id_no serial primary key, 
	incident_no int not null check(incident_no between 1000000 and 999999999),
	category text not null check(length(category) > 0),
	dayOfWeek text check(length(dayOfWeek) between 5 and 10),
	mmddyyyy date,
	timeofday time, 
	pdDistrict text,
	address text,
	x_coord double precision,
	y_coord double precision
);

grant select, insert, update, delete on table Crime_Reports to "LU.ENGI3675";
grant select, insert, update, delete on table Crime_Reports to "DefaultAppPool";

drop table if exists Crime_Reports_Indexed cascade;

create table Crime_Reports_Indexed(
	id_no serial primary key, 
	incident_no int not null check(incident_no between 1000000 and 999999999),
	category text not null check(length(category) > 0),
	dayOfWeek text check(length(dayOfWeek) between 5 and 10),
	mmddyyyy date,
	timeofday time, 
	pdDistrict text,
	address text,
	x_coord double precision,
	y_coord double precision
);

grant select, insert, update, delete on table Crime_Reports_Indexed to "LU.ENGI3675";
grant select, insert, update, delete on table Crime_Reports_Indexed to "DefaultAppPool";

create index on crime_reports_indexed (y_coord);