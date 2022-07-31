# Hotelli broneerimise süsteem

Lihtne hotellitubade otsimise ja broneerimise ning haldamise süsteem. 

Avalehel on võimalik otsida vabu hotelli tubade tüüpe ajaperioodil.  Sobiva toa leidmise korral saab teha sellele broneeringu. Broneeringuid saab hiljem broneeringu tigije emaili aadressi kasutades otsida ning neid muuta või tühistada kuni 3 päeva enne broneeringu algust.

Hotelli töötajad saavad sisse logida hotelli administreerimise lehele. Selleks on võimalik luua uus kasutaja ning hetkel on testimise eesmärkidel registreerimine avatud kõigile soovijatele. Hetkel on andmebaasi sisestatud üks testhotell ning uute hotelli lisamine ei ole kliendipoolses rakenduses lubatud, kuid serveripoolne rakendus toetab ka mitme hotelli kasutamist. Hotelli administreerimise vaates on võimalik muuta hotelli andmeid, lisada ruumitüüpe, ruumide lisasid ja ruume ning neid vajadusel kustutada. Samuti saab näha kõiki tehtud broneeringuid ning neid vajadusel muuta või tühistada. Süsteem ei luba broneerimisel kindlale ajaperioodile ruumitüübile teha rohkem broneeringuid kui on hotellis selle ruumitüübiga tube. 

Serveripoolne rakendus on kirjutatud kasutades .NET 6 raamistikku ning kliendirakendus kasutab Vue 3 raamistikku. API dokumenteerimiseks on kasutatud Swaggerit, mille avaleht avaneb serveripoolse rakenduse kompileerimisel (asukoht: localhost:7009/index.html).

Kliendirakenduse käivitamiseks on vajalik kõigepealt installeerida kõik kasutatavad moodulid:

~~~bash
npm install
~~~

Seejärel on võimalik käivitada testserver, mis kasutab vaikimisi porti 3000:

~~~bash
npm run dev
~~~

Süsteem kasutab andmebaasina PostgreSQL andmebaasi, mis on hoiustatud välise teenusepakkuja juures ning seetõttu lisaseadistamist ei vaja.