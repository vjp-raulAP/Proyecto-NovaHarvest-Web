### 1. De .cshtml a HTML.
**Explica qué le pasa a Surplus.cshtml desde que haces dotnet
build hasta que el navegador recibe el HTML. Compila con dotnet build
-p:EmitCompilerGeneratedFiles=true, busca la clase generada para Status.cshtml y
copia un WriteLiteral y un Write: ¿de qué parte de tu .cshtml sale cada uno? ¿Por qué
no hay ningún .cshtml dentro del contenedor? ¿En qué ensamblado acaban las páginas, y en cuál acababan antes de .NET 6?**

Al ejecutar el comando `dotnet build -p:EmitCompilerGeneratedFiles=true`, el proyecto se compila y se genera el archivo `NovaHarvest.Web.dll`.

![Archivos generados por Razor](img/cap1.png)

Durante la compilación, Razor transforma los archivos `.cshtml` en código C# y los compila. Para comprobarlo, he buscado los archivos generados dentro de la carpeta `obj/Debug/net10.0/generated`. Allí he encontrado las páginas con extensión `.g.cs`, como `Surplus_cshtml.g.cs` y `Status_cshtml.g.cs`.
![Archivos generados por Razor](img/cap2.png)
Dentro de estas clases generadas aparecen los métodos `WriteLiteral()` y `Write()`.

El `WriteLiteral()` sale directamente del contenido HTML que he escrito en el archivo `.cshtml`, como las etiquetas y los textos.

El `Write()` sale de las expresiones Razor que empiezan por `@`, como `@Model.ServerTime.ToString("dd/MM/yyyy HH:mm:ss")`, y muestra el valor obtenido de esa expresión.
![WriteLiteral](img/cap3.png)
Cuando el navegador solicita una página, ASP.NET Core ejecuta el código compilado de Razor y genera el HTML que finalmente recibe el navegador.

No se ven `.cshtml` dentro del contenedor porque las páginas Razor ya se han transformado y compilado. La aplicación además usa el código compilado para ejecutarse, por lo que no necesita los archivos `.cshtml` originales.


**2. Tecnologías. Escribe la línea Precio de excedente: ... de tu página como se
escribiría en PHP y en ASP clásico, y compárala con la tuya en Razor: ¿cómo marca cada
una dónde empieza y dónde acaba el código? ¿Qué has ganado al pasar de los MapGet de
UT2 a Razor Pages? Compara tu Program.cs de antes y de ahora. Añade también esa línea escrita en ERB (Ruby on Rails).**
### 2. Tecnologías

En mi proyecto, la línea del precio en Razor es:

```cshtml
<p>Precio de excedente: @Model.PrecioExcedente.ToString("C") € / kg</p>
```

En PHP se utiliza:

```php
<p>Precio de excedente: <?php echo $precioExcedente; ?> € / kg</p>
```

En ASP clásico:

```asp
<p>Precio de excedente: <%= precioExcedente %> € / kg</p>
```

En ERB (Ruby on Rails):

```erb
<p>Precio de excedente: <%= @precio_excedente %> € / kg</p>
```

Cada tecnología utiliza diferentes marcas para indicar dónde empieza y termina el código. Razor utiliza `@` para introducir expresiones C#.

Al pasar de los `MapGet` de UT2 a Razor Pages he conseguido organizar mejor el proyecto, separando el HTML en archivos `.cshtml` y la lógica C# en archivos `.cshtml.cs`.
Así lo tenía antes las Páginas HTML
![HTML sin Razor](img/cap4.png)

Además, `Program.cs` ahora es más sencillo, porque solo necesita configurar los servicios y las rutas de Razor Pages.

![program.cs ahora](img/cap5.png)


### 3. Etiquetas de código.
 Copia de tu proyecto un ejemplo de cada: expresión implícita,
expresión explícita, bloque @{ }, @@ y comentario @* *@, junto con el HTML que genera
cada uno (código fuente de la página). ¿Por qué info@novaharvest.es no necesita @@ y
@novaharvest sí?**

**a. Expresión implícita**

Se utiliza `@` seguido de una expresión C#. En `Pages/Status.cshtml`, en una línea que muestra una propiedad del modelo mediante `@Model`. 
```cshtml
<p>Servidor: @Model.ServerName</p>
<p>Visitas desde el arranque: @Model.Visits</p>
```

![/status](img/cap6.png)

El HTML generado sería este.

![HTML de /status](img/cap7.png)

**b. Expresión Explícita**
 Hay que localizar en los archivos `.cshtml` una expresión escrita con `@(...)`. En el proyecto pongo como ejemplo. en /Pages/Surplus.cshtml

Código Razor:

```cshtml
<p>Nota del productor: @Html.Raw(Model.Excedente.Note ?? "Sin observaciones")</p>
```
![/Surplus](img/cap8.png)

El HTML generado sería.

![HTML de /Surplus](img/cap9.png)

**c. Bloque de código**
 
 Cada vez que he usado  @{ ---} se ejecuta como bloque de código.

 por ejemplo,
 ```cshtml
@{
    ViewData["Title"] = "Estado del servidor";
}
```
captura en el proyecto 
![alt text](img/cap10.png)

**d. caracter @@**
En el proyecto lo he usado en "About.cshtml" por ejemplo. es para  escapar la @.
```cshtml
 <p>Escríbenos a info@novaharvest.es o síguenos en @@novaharvest</p>
```

El el proyecto queda así

![/About](img/cap11.png)

El Html quedaría 

![/About](img/cap12.png)

**d. comentarios Razor**

Los comentarios en Razor son (@*   *@). En mi proyecto tengo un comentario por ejemplo en /Surplus
```cshtml
 @* boton aparece deshabilitado cuando el producto está agotado*@
<button disabled="@(!Model.PuedeReservar)">Reservar</button>

```

![/Surplus](img/cap13.png)

Comprobacion que no se ve el comentario.

![/Surplus](img/cap14.png)

**¿Por qué info@novaharvest.es no necesita @@ y @novaharvest sí?**
En `info@novaharvest.es`, el símbolo `@` está precedido por texto, así que Razor lo reconoce como parte de una dirección de correo. Sin embargo, en `@novaharvest` aparece al principio y Razor puede confundirlo con una expresión de C#. Por eso se escribe `@@` cuando queremos mostrar una arroba sin que Razor la interprete como código.


### 4. Sintaxis.
 Haz estos dos cambios en Surplus.cshtml (y deshazlos después): escribe
@Model.SurplusPrize en lugar de @Model.SurplusPrice, y escribe
@Model.Surplus.PricePerKg * 0.7m en alguna línea. Copia lo que ocurre en cada caso
(el error de dotnet build o el HTML generado). ¿Por qué el primero no compila y el
segundo sí, aunque el resultado esté mal? ¿Cómo se corrige el segundo?

Al hacer la sustitución que el enuciado pide, cambio ` <p>Precio con descuento: @Model.PrecioExcedente.ToString("C")</p>` por `<p>Precio con descuento: @Model.SurplusPrize.ToString("C")</p>`ya que no usé el mismo nombre de las variables. Al ejecutar `dotnet run`, me genera  `error CS1061: "SurplusModel" no contiene una definición para "SurplusPrize" ni un método de extensión accesible "SurplusPrize" que acepte un primer argumento del tipo "SurplusModel" (¿falta alguna directiva using o una referencia de ensamblado?)`.  Este error al compilar debido a que  al buscar el nombre de la variable definida en `Surplus.cshtml.cs`no encuentra esa variable.

![SurplusPrize](img/cap15.png)

Si añadimos en mi caso `@Model.Excedente.PricePerKg * 0.7m` como prueba  en la linea justo debajo de lo que he probado anteriormente. Ejecuto `dotnet build`y me compila sin problemas. 
![Excedente.PricePerKg](img/cap16.png)

Compruebo el HTML generado y me aparece en Excedente `Prueba pregunta4: 3,20 * 0.7` debido a que Razor  lo separa en 2. Según los apuntes. primero coge como C# `@Model.Excedente.PricePerKg` y luego coge  la multiplicación con texto normal `*0.7m`. Por ese motivo no realiza la multiplicación en este caso. Para corregirlo sería metiendolo todo entre parentesis `@(Model.Excedente.PricePerKg * 0.7m)`

![Excedente.PricePerKg](img/cap17.png)

Quedaría así Corregido 
![Excedente.PricePerKg](img/cap18.png)

![Prueba excedente](img/cap19.png)


### 5. Efectos en el documento. 
Compara cómo se ve la nota del productor en pantalla y en el
código fuente de /surplus. ¿Qué ha hecho Razor y por qué? ¿Qué podría pasar si usaras
@Html.Raw y la nota la escribiera un usuario malintencionado? 

Lo primero que hago es hacer la prueba sin `@Html.Raw` y debajo  con `@Html.Raw` el resultado obtenido es que en el primero, Razor muestra la nota pero convierte los caracteres especiales en HTML para evitar que se interpreten como código HTML. En la segunda opción `<p>Nota del productor: @Html.Raw(Model.Excedente.Note ?? "Sin observaciones")</p>` . Html.Raw muestra el contenido sin realizar esa conversion. Por eso el navegador puede interpretarlas.

![Html.raw](img/cap20.png)


**¿Qué podría pasar si utilizáramos @Html.Raw y la nota la escribiera un usuario malintencionado?**
Un usuario podría introducir código HTML o JavaScript malicioso. Al utilizar Html.Raw, ese código podría ejecutarse en el navegador de otros visitantes, provocando un ataque XSS (Cross-Site Scripting).

**Copia el elemento de los días y el botón en los escenarios A, C y D, y explica por qué a veces el atributo class o**
**disabled no aparece.**

**Escenario A** No se modifica ningún dato. La página debe mostrar el resultado inicial.

![escenario a](img/cap21.png)

Comprobacion


![escenario a](img/cap22.png)


![escenario a](img/cap23.png)

Esto ocurre porque Razor tiene un comportamiento especial: cuando el valor de un atributo HTML es null, no genera ese atributo.


**Escenario C:** 
ReservedKg = 343. Los kilos disponibles son 0, no hay cajas ni kilos sueltos, el valor del excedente es 0,00 €, el estado es «Agotado» y el botón aparece deshabilitado.

![escenario c](img/cap24.png)

Comprobación


![escenario c](img/cap25.png)



![escenario c](img/cap26.png)

El boton aparece desabilitado y en nos aparece en en DevTools `<button disabled="disabled">Reservar</button>` porque no quedan kilos.


**escenario D** . Cambiar la fecha de cosecha para que sea de hace 6 días.

![escenario d](img/cap27.png)

Comprobación

![escenario d](img/cap28.png)
![escenario d](img/cap29.png)

Cuando el valor de la expresión es null, Razor no muestra el atributo `class="urgent"`.Lo mismo ocurre con `disabled` cuando su valor es `false`.Como sólo queda un día para consumir el producto y todavía quedan existencias.

### 6. Directivas.
 Haz una tabla con cada directiva o fichero especial de tu proyecto (@page, @page "/ticket", @model, @using, @namespace, @inject, _ViewStart.cshtml y Layout = null) y el comportamiento por defecto que cambia. ¿Por qué /reservations/ticket da 404?

**Directivas y archivos de Razor Pages**

| **Directiva o fichero** | **comportamiento** |
| ----------------------- | ----------------------- |
| `@page` | La utilizo para indicar que ese archivo puede funcionar como una página web y tener una dirección propia. |
| `@page "/ticket"` | Me permite poner una dirección concreta a la página. Así puedo acceder a ella escribiendo `/ticket` en el navegador. |
| `@model` | La utilizo para enlazar el HTML con el archivo C# que contiene los datos y las operaciones de esa página. |
| `@using` | Me sirve para incluir las clases de otros espacios de nombres y poder utilizarlas directamente. |
| `@namespace` | Indica a qué espacio de nombres pertenece la página que Razor crea automáticamente al compilarla. |
| `@inject` | Me permite acceder desde la página a un servicio que ya proporciona ASP.NET Core, sin necesidad de crear una instancia por mi cuenta. |
| `_ViewStart.cshtml` | Es un archivo común que se ejecuta al cargar las páginas y permite indicar qué diseño general van a compartir. |
| `Layout = null` | Lo utilizo cuando quiero que una página se muestre por separado, sin incluir la estructura general del resto de la web. |


 **Quita @namespace de _ViewImports.cshtml: copia el error y explícalo.**

 ![pregunta 6](img/cap30.png)
 ![pregunta 6](img/cap31.png)

 Al quitar @namespace de _ViewImports.cshtml, Razor deja de utilizar el espacio de nombres que teníamos definido para las páginas. Como consecuencia, puede generar las clases de las páginas en un espacio de nombres diferente y no encontrar sus correspondientes clases PageModel. Pr ese motivo salta el `error CS0246`

  **¿Por qué /reservations/ticket da 404?**

  ![pregunta 6](img/cap32.png)

 Porque en /Ticket.cshtml tengo configurado `@page "/ticket"`con lo que la URL donde Razor va a renderizar la Página es `localhost:8080/Ticket` y ponemos otra ruta qno sea esa mnos dará el `error 404`

![pregunta 6](img/cap33.png)
 

### 7. Tipos y operadores.
 Haz una tabla con cada dato que calcula SurplusModel: su tipo de C#, los operadores que usa y por qué ese tipo. ¿Por qué RemainingKg / BoxKg da cajas completas? ¿Qué pasaría si los kilos fueran decimal? ¿Qué escribe Razor si muestras directamente un bool? ¿Y un null?

