# MasterEDI.CFDI

## A. Arquitectura y Patrones en CFDI

### Inversión de Dependencias (DIP)

**Pregunta:**
> ¿Cómo el Principio de Inversión de Dependencias (DIP) permite que su `CfdiService` (en la capa *Application*) dependa de la interfaz `ICertificadoService` (en la capa *Domain*) en lugar de una implementación concreta (en la capa *Infrastructure*)?

**Respuesta:**
>Se utiliza la abstracción como CfdiService no conoce la clase especifica que maneja los certificados solo va a conocer la interfas, esto permite que pueda ser
  instanciado dentro de las pruebas unitarias

---

### Principio de Responsabilidad Única (SRP)

**Contexto:**
La generación de un CFDI implica tres pasos principales:
1. Generar la Cadena Original  
2. Generar el Sello Digital  
3. Generar el XML final

**Pregunta:**
> ¿Cómo distribuyó estas tres responsabilidades en diferentes clases o métodos para cumplir con el SRP?  
> Explique por qué el sello debe ser una clase separada de la cadena original.

**Respuesta:**
>Estas clases se separan para que puedan ser modificadas dependiendo de la nececidad,
>
>1 Para generar la cadena original, implica que existe una normativa sei se agregan o se quitan o se validan cambos la cadena y el orden de los objetos tiende a >cambiar.
>
>2 El sello es otro proceso que puede cambiar por separado, los metodos de cifrado, tipo de hash etc...
>
>3 El XML tambien puede cambiar, basicamente en la estructura de los nodos, no tienen que cambiar los anteriores para que exista un cambio en el XML.

---

## B. Operaciones, Rendimiento y Seguridad (CFDI)

### Procesamiento Asíncrono y Resiliencia:

**Pregunta:**
>Si la llamada al servicio de Timbrado del PAC tarda 10 segundos, ¿Cómo utilizaría la programación Asíncrona (async/await) en .NET para liberar hilos y evitar cuellos de botella en la API, permitiendo a otros usuarios facturar mientras el primero espera la respuesta del PAC?

**Respuesta:**
>De entrada, basado en un modelo de asignación de tickets, para las respuestas asincronas en la primera petición se reciben los datos y se da el OK,
diciendole que vaya al metodo de estatus, mientras tanto internamente se ejecutan los procesos necesarios para finalizar la operación, el estatus, 
debera informar en que estado se encuentra el request inicial y solo eso, cuando pase el estatus a finalizado, entonces se pasará al siguietne request,
que devolverá la información solciitada, otras maneras de manejar los reintentos del procesamiento seria usando polly, una herramienta de C# que mermite 
gestionar los reintentos eficientemente

### Seguridad de Certificados (CSD):

**Pregunta:**
>El CSD es la firma electrónica de la empresa.
¿Por qué es una mala práctica cargar la contraseña de la clave privada (*.key) directamente desde un archivo en el disco duro del servidor web, y cómo una implementación correcta de ICertificadoService debería interactuar con un servicio como Azure Key Vault (o HashiCorp Vault) para mitigar este riesgo?

**Respuesta:**
>Creo que esto viola los principios de seguirdad de PCI y de OWASP, y respecto a Azure Key Valult realmente solo las aplicaciones permitidas tendrian accesos sobre 
la información, es un modo más seguro.
