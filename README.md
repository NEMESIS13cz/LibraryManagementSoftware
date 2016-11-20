# LibraryManagementSoftware

Just a school project :D

-----

## Klient

Grafický program pro čtenáře i administrátory. Běžný uživatel (čtenář) si může prohlížet, vyhledávat a rezervovat knihy. Pokud se přihlásí administrátor, program se automatický otevře v pokročilém okně, které umožńuje přidávat, odebírat a upravovat knihy, čtenáře a jiné administrátory.

Je také možné se nepřihlašovat a knihy pouze prohlížet jako návštěvník.

Před přihlášením lze změnit IP adresu a port serveru.

Pokud uživatel chce změnit údaje o svém účtu, musí zajít za administrátorem.

## Server

Server je konzolová aplikace určená pro běh na serverovém počitači knihovny. Obsahuje databázi knih a uživatelů, konfigurační složky a logování veškerých interakcí s databází (modifikace, odebrání a přidání knih/uživatelů) a příchozích připojení.

#### Konfigurace

Při prvním spuštění serveru se vytvoří výchozí konfigurace *library.cfg*:
```
port=15410
admin_pass=password1337
```
**port** je číslo portu, na kterém bude server očekávat připojení.
**admin_pass** je heslo k výchozímu administrátorskému účtu **admin**, heslo doporučujeme okamžitě změnit. Výchozí účet **admin** bude znovu vytvořen při restartu serveru pokud je někdy smazán.

#### Log událostí

Log složky se ukládají ve formátu *yyyy-mm-dd-č.log*, kde *yyyy* je rok, *mm* měsíc, *dd* den a *č* pořadové číslo logu v den, kdy byl log vytvořen.

#### Databáze

Při prvním spuštění serveru se vytvoří SQLite databáze *database.sql*, obsahuje všechny knihy a uživatele.
