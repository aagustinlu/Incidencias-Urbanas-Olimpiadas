function guardarSesion(u){
  try{ localStorage.setItem('moroncito_sesion', JSON.stringify(u)); }
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

document.getElementById('formLogin').addEventListener('submit', async e => {
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

  const btn = document.querySelector('.login-btn');
  if(btn){ btn.disabled = true; btn.textContent = 'Ingresando...'; }

  try{
    const resp = await fetch('/api/usuarios/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ dni, nombre, apellido, barrio, password: pass })
    });

    if(resp.status === 401){
      errPass.textContent = 'Contraseña incorrecta. Volvé a intentarlo.';
      errPass.style.display = 'block';
      document.getElementById('loginPass').value = '';
      document.getElementById('loginPass').focus();
      return;
    }

    if(!resp.ok){
      errPass.textContent = 'No se pudo conectar con el servidor. Probá de nuevo.';
      errPass.style.display = 'block';
      return;
    }

    // La sesión ahora viene del backend (incluye el usuarioId real de la base)
    const sesion = await resp.json();
    guardarSesion(sesion);
    window.location.href = 'index.html';
  }catch(err){
    console.error(err);
    errPass.textContent = 'No se pudo conectar con el servidor. Probá de nuevo.';
    errPass.style.display = 'block';
  }finally{
    if(btn){ btn.disabled = false; btn.textContent = 'Ingresar'; }
  }
});
