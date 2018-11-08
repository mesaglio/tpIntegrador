# Diseño de Sistemas - En casa compilaba

[Diagrama de Clases](https://www.lucidchart.com/documents/edit/ceb23ed0-1b33-418a-add7-bd27eacf69dc/1)

Completar el [Excel de Justificaciones](https://drive.google.com/open?id=1KMen2VzRO-dwepk3WMq4hhfEXBdTLhqMDdQSORsywe4)

## Correcciones para la entrega 4
 
### Abrir las reglas, para evaluar si es mayor o menor a la magnitud, no solo ==.
- [x]  ### Sacar el boton que esta en cliente cargar transformadores, y agregarlo al Administrador.
### Consumo por hogar en cliente.
### Llamar desde el controles a los modelos.

## Entregas

### 1

- [x] Diagrama/s de clases actualizado (en el caso de que 
se utilice algún patrón de diseño, marcarlo en el diagrama).

- [x] Proponer y documentar de qué manera puede darse la comunicación entre el sistema y los dispositivos, sin detalles de implementación. Evaluar su impacto sobre el modelo de objetos.

---

### 2

- [x] Diagrama de clases, actualizado.

- [x] Modelado de dispositivos.

- [x] Modelado de transformadores y geoposicionamiento.

---

### 3

- [x] Diagrama de clases actualizado.

- [x] Diagrama entidad relación (DER), FK, PK y las restricciones según corresponda.

- [x] Documentación de configuración general: Documento que especifique cómo configurar la base de datos y como cargar datos de prueba en ella.

Casos de prueba:

- [x] Crear 1 usuario nuevo. Persistirlo. Recuperarlo, modificar la geolocalización y grabarlo. Recuperarlo y evaluar que el cambio se haya realizado.
- [x] Caso de prueba 2: Recuperar un dispositivo. Mostrar por consola todos los intervalos que estuvo encendido durante el último mes. Modificar su nombre (o cualquier otro atributo editable) y grabarlo. Recuperarlo y evaluar que el nombre coincida con el esperado.
- [ ] Caso de prueba 3: Crear una nueva regla. Asociarla a un dispositivo. Agregar condiciones y acciones. Persistirla. Recuperarla y ejecutarla. Modificar alguna condición y persistirla. Recuperarla y evaluar que la condición modificada posea la última modificación.
- [x] Caso de prueba 4: Recuperar todos los transformadores persistidos. Registrar la cantidad.
- [ ] Caso de prueba 5: Mostrar por periodo el consumo total del hogar.

---

### 4

- [ ] Diagrama de arquitectura.

- [ ] Documento de diseño de interfaz: el documento deberá especificar de forma clara: tecnologías utilizadas, URLs, pantallas y la navegación entre ellas.

- [ ] Implementacion de pantallas enunciadas: Login, Administrador, Mapas y Cliente.

