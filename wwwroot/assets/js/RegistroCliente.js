// RegistroCliente.js

$(document).ready(function () {
    // Al hacer clic en el botón "Enviar"
    $("#verificarNumero").click(function () {
        // Aquí, envía el número de teléfono al controlador de Twilio y realiza la verificación
        var numeroCelular = $("#tuCampoDeCelular").val(); // Reemplaza "tuCampoDeCelular" con el ID de tu campo de entrada de celular
        $.post("/Twilio/EnviarCodigoVerificacion", { numeroCelular: numeroCelular }, function (response) {
            if (response.status === "pending") {
                // Si la verificación está pendiente, muestra el modal
                $("#modalCodigoVerificacion").modal("show");
            } else {
                // Manejar otros casos aquí, como si la verificación falla
            }
        });
    });

    // Al hacer clic en el botón "Guardar" en el modal
    $("#guardarRegistro").click(function () {
        var codigoVerificacion = $("#codigoVerificacion").val();
        // Aquí, envía el código de verificación al controlador para verificarlo
        $.post("/Twilio/VerificarCodigo", { codigoVerificacion: codigoVerificacion }, function (response) {
            if (response.status === "verified") {
                // Si la verificación es exitosa, habilita la segunda sección
                $("#seccion2").show();
                $("#modalCodigoVerificacion").modal("hide");
            } else {
                // Manejar otros casos aquí, como si la verificación falla
            }
        });
    });
});
