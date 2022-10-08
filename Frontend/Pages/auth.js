// alert("hola")
const BACKEND_URL = "http://localhost:1337/";
const email = document.querySelector("#email");
const password = document.querySelector("#password");
const login = document.querySelector("#login");

var form = document.getElementById("myForm");

function decodeToken(response={}){
    return JSON.parse(window.atob(response?.data?.jwt?.split(".")[1]))
}

let config = {
    headers: {
        "Content-Type": "application/json",
    },
  };

function checkTokenDateValidity(expirationDate = false){
  if (false) return false;
  //cuantos milisegundos de enero de 1970 han transcurrido
  const today = new Date().getTime();
  const expirDate = new Date(expirationDate * 1000).getTime();
  return today < expirDate;
}
//funcionalidad de loguear
form.addEventListener("submit", function(event) {
  event.preventDefault();
   if (!email.value || !password.value) {
    swal.fire("llena campos");
  } else {
    const body = {
      identifier: email.value,
      password: password.value,
    };
      axios.post(BACKEND_URL+"auth/local", body, config).then(response => {
      const {iat, exp} = decodeToken(response);
      //probar si la fecha del token es valido
      const isValid = checkTokenDateValidity(exp);
      localStorage.setItem("session", response.data.jwt)
      const usuario = response?.data?.user;
      localStorage.setItem("usario",JSON.stringify(usuario))
      if (isValid && localStorage.getItem("session")){
        location.replace("FiltroBienvenida.html")
      }
    }).catch(error => {
      Swal.fire("clave incorrecta verificar por favor")
    });
  }
})

function handleForm(event) { event.preventDefault(); } 
form.addEventListener('submit', handleForm);

var serializeForm = function (form) {
	var obj = {};
	var formData = new FormData(form);
	for (var key of formData.keys()) {
		obj[key] = formData.get(key);
	}
	return obj;
};

function validateEmail(email) {
    const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}
const registroForm = document.querySelector("#registroForm");
var data = new FormData(registroForm);

registroForm.addEventListener('submit', (event)=>{
    event.preventDefault();
    const bodyData = serializeForm(event.target)
    console.log("🚀 ~ file: auth.js ~ line 71 ~ registroForm.addEventListener ~ bodyData", bodyData)
     const isValid = validateEmail(bodyData.correo);
    if (bodyData.usuario == false || bodyData.contraseña == false){
        Swal.fire("Falto llenar los campos")
    } 
    else if (!isValid) {
        Swal.fire("correo invalido")
    } else {
        const body = {
            "username":bodyData.usuario,
            "email":bodyData.correo,
            "password":bodyData.contraseña
          };
        axios.post(BACKEND_URL+"auth/local/register", body, config).then(response => {
            //probar si la fecha del token es valido
            Swal.fire("Registro exitoso inicie sesion")
          }).catch(error => {
            Swal.fire("Error de registro")
          });
    }
    
})