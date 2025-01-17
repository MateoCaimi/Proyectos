import MenuHeader from "./MenuHeader"
import logo from '../imagenes/logoultimo.png'
import styles from './css/Header.module.css'

const Header = () => {
    return (

        <header className={styles.header} >
            <figure className={styles.figure}>
                <img className={styles.img} src={logo} width='100px' alt="logo" title="Logo Potencia vial"></img>
            </figure>
            <div className={styles.div}>
            <MenuHeader  />
            </div>
        </header>
        )
}

export default Header