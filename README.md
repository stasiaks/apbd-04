# Komentarze

## Edycja zwierzęcia

Zgodnie z opisem zadania, edycja została zaimplementowana jako `PUT`.
Nie jest to jednak stricte mówiąc "edycja", zachowanie zgodności z RFC 9110 oznacza, że można tym endpointem zwierzę utworzyć od zera.
Sama edycja wymagała by operacji `PATCH`, np. JSON Patch (RFC 6902).
Natomiast praca z JSON Patch jest daleka od przyjemnej, więc cieszy mnie, że nie było to treścią zadania.