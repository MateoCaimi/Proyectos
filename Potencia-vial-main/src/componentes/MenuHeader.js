import React from 'react'
import { Link } from 'react-router-dom'
import styles from './css/MenuHeader.module.css'

const MenuHeader = () => {
    return (
        <nav className={styles.nav}>
            <ul className={styles.ul}>
                <li className={styles.li}>
                    <Link to={''} className={styles.link}>Buscar</Link>
                </li>
                <li className={styles.li}>
                    <Link to={''} className={styles.link}>Vender</Link>
                </li>
                <li className={styles.li}>
                    <Link to={''} className={styles.link}>Contacto</Link>
                </li>
            </ul>
        </nav>
    )
}

export default MenuHeader