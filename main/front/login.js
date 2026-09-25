function cargarUsuarios(){
  try{ return JSON.parse(localStorage.getItem('moroncito_usuarios') || '[]'); }
  catch(e){ return []; }
}
function guardarUsuarios(lista){
  try{ localStorage.setItem('moroncito_usuarios', JSON.stringify(lista)); }
  catch(e){ console.error('No se pudo guardar usuarios', e); }
}
function guardarSesion(u){
  try{ localStorage.setItem('moroncito_sesion', JSON.stringify({dni:u.dni, nombre:u.nombre, apellido:u.apellido, barrio:u.barrio})); }
  catch(e){ console.error('No se pudo guardar la sesión', e); }
}
function cargarSesion(){
  try{ return JSON.parse(localStorage.getItem('moroncito_sesion') || 'null'); }
  catch(e){ return null; }
}

// Si ya hay sesión activa, va directo a la app
if(cargarSesion()){
  window.location.href = 'index.html';
}

document.getElementById('formLogin').addEventListener('submit', e => {
  e.preventDefault();
  const dni = document.getElementById('dni').value.trim();
  const nombre = document.getElementById('loginNombre').value.trim();
  const apellido = document.getElementById('loginApellido').value.trim();
  const barrio = document.getElementById('loginBarrio').value.trim();
  const pass = document.getElementById('loginPass').value;
  const errPass = document.getElementById('errPass');

  const dniValido = /^\d{7,8}$/.test(dni);
  document.getElementById('errDni').style.display = dniValido ? 'none' : 'block';
  document.getElementById('errNombre').style.display = nombre ? 'none' : 'block';
  document.getElementById('errApellido').style.display = apellido ? 'none' : 'block';
  document.getElementById('errBarrio').style.display = barrio ? 'none' : 'block';
  errPass.textContent = 'Ingresá tu contraseña.';
  errPass.style.display = pass ? 'none' : 'block';
  if(!dniValido || !nombre || !apellido || !barrio || !pass) return;

  const usuarios = cargarUsuarios();
  const existente = usuarios.find(u => u.dni === dni);

  if(existente){
    if(existente.password !== pass){
      errPass.textContent = 'Contraseña incorrecta. Volvé a intentarlo.';
      errPass.style.display = 'block';
      document.getElementById('loginPass').value = '';
      document.getElementById('loginPass').focus();
      return;
    }
    if(!existente.barrio){
      existente.barrio = barrio;
      guardarUsuarios(usuarios);
    }
    guardarSesion(existente);
  } else {
    const nuevo = { dni, nombre, apellido, barrio, password: pass };
    usuarios.push(nuevo);
    guardarUsuarios(usuarios);
    guardarSesion(nuevo);
  }

  window.location.href = 'index.html';
});