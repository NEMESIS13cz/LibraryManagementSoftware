# LibraryManagementSoftware

Just a school project :D

-----

## Klient

#### **Knihovníci**
#### Instalace
Instalace je velmi jednoduchá. Soubor LibrarySoftware.exe přetáhněte do jakékoliv složky kdekoliv na počítači a případně si udělejte zástupce na plochu.
#### Přihlášení
Při prvním přihlášení je potřeba kliknout na Input IP address a zde následně zadat správnou IP adresu a port, který si knihovna zvolila. Po uložení si tuto volbu program bude pamatovat a tudýž není potřeba tento krok opakovat.
Výchozí přihlašovací údaje jsou – E-mail: admin Heslo: password1337
Doporučujeme smazat výchozí údaje, ovšem z důvodu funkčnosti programu v případě vypnutí serveru se po jeho zapnutí znovu vytvoří!
#### Použití
#### *Hledání*
Hledat můžete ve všech třech oddílech. Vždy zadáte podle, kterého kritéria hledáte např. podle jména a napíšete jak se daná věc/daný člověk jmenuje*. Výsledek se vám násladně zobrazí.
*Pozn.: Stačí zadat i část  názvu a objeví se všechny, které to obsahují. (Posouvat mezi výsledky můžete pomocí malých šipek nad seznamem)

#### *Obsluha čtenářů*
Zadáním nového čtenáře mu rovnou nastavíte přihlašovací údaje k tomuto programu a on si může prohlédnout knihy a připadně si je i rezervovat, pokud nejsou půjčené nebo rezervované. Heslo změníte tím, že při úpravě kliknete na tlačítko Změnit heslo a následně uložíte změnu.
Půjčením rezervované knihy se automaticky daná rezervace zruší a ta kniha je "pouze" půjčená.
Kliknutím na tlačítko Zrušit rezervaci se automaticky zruší veškeré rezervace uživatele! 
Dobu vypůjčení a rezervací tento program nijak nehlídá!

#### *Půjčení knihy*
Půjčit knihu lze pouze ze seznamu čtenářů (Čtenáři>Půjčit knihu). Knihu naleznete a už jen jednoduše zmáčknete půjčit. Program i sám zkontroluje, jestli ji už nekdo jiný nemá půjčenou, v tom případě vypůjčení nebude možné.

#### **Čtenáři**
#### Instalace
Instalace je velmi jednoduchá. Soubor LibrarySoftware.exe přetáhněte do jakékoliv složky kdekoliv na počítači a případně si udělejte zástupce na plochu.
#### Přihlášení
Při prvním přihlášení je potřeba kliknout na Input IP address a zde následně zadat správnou IP adresu a port, který si knihovna zvolila. Po uložení si tuto volbu program bude pamatovat a tudýž není potřeba tento krok opakovat.

Heslo jste si vybral nebo vám knihovna vybrala při zápisu.
#### Použití
#### *Hledání*
Hledat můžete ve všech třech oddílech. Vždy zadáte podle, kterého kritéria hledáte např. podle jména a napíšete jak se daná věc/daný člověk jmenuje*. Výsledek se vám násladně zobrazí.
*Pozn.: Stačí zadat i část  názvu a objeví se všechny, které to obsahují. (Posouvat mezi výsledky můžete pomocí malých šipek nad seznamem)*
#### *Rezervace*
Po přihlášení máte možnost projíždět v seznamu knih, které má knihovna k dispozici a rezervovat si knihy, které si chcete půjčit*.

*Pozn.: U knih, u kterých jste provedl rezervaci, ji můžete i sám zrušit.*

## Server

Server je konzolová aplikace určená pro běh na serverovém počitači knihovny. Obsahuje databázi knih a uživatelů, konfigurační složky a logování veškerých interakcí s databází (modifikace, odebrání a přidání knih/uživatelů) a příchozích připojení.

#### **Konfigurace**

Při prvním spuštění serveru se vytvoří výchozí konfigurace *library.cfg*:
```
port=15410
admin_pass=password1337
```
**port** je číslo portu, na kterém bude server očekávat připojení.
**admin_pass** je heslo k výchozímu administrátorskému účtu **admin**, heslo doporučujeme okamžitě změnit. Výchozí účet **admin** bude znovu vytvořen při restartu serveru pokud je někdy smazán.

#### **Log událostí**

Log složky se ukládají ve formátu *yyyy-mm-dd-č.log*, kde *yyyy* je rok, *mm* měsíc, *dd* den a *č* pořadové číslo logu v den, kdy byl log vytvořen.

#### **Databáze**

Při prvním spuštění serveru se vytvoří SQLite databáze *database.sql*, obsahuje všechny knihy a uživatele.
