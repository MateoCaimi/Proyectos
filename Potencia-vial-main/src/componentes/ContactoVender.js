import style from './css/ContactoVender.module.css'
const ContactoVender = () => {
    return (
        <div>
            <h3 className={style.h}>Estás buscando vender tu maquina?</h3>
            <p>Nosotros te ayudamos</p>
            <input type='button' value='Contactanos' className={style.button}></input>
        </div>
    )
}

export default ContactoVender