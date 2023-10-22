const mysql = require("mysql");
const config = require("../config/config");

const dbConfig = {
  host: config.host,
  user: config.user,
  password: config.password,
  database: config.database,
};

let db;

async function conMySql() {
  db = mysql.createConnection(dbConfig);

  db.connect((err) => {
    if (err) {
      console.log("[db err]", err);
      console.log("Entro aca");
    } else {
      console.log("DB conectada!");
    }
  });

  db.on("error", (err) => {
    console.log("[db err]", err);
    if (err.code === "PROTOCOL_CONNECTION_LOST") {
      conMySql();
    } else {
      throw err;
    }
  });
}

async function getMaquinas() {
  await conMySql();
  return new Promise((resolve, reject) => {
    db.query("SELECT * from potencia_vial.Maquina", (err, results) => {
      if (err) {
        console.error("Error en la consulta: " + err);
        reject(err);
      } else {
        resolve(results);
      }
    });
  });
}

async function getMaquina(id) {
  await conMySql();
  return new Promise((resolve, reject) => {
    db.query(
      `SELECT * from potencia_vial.Maquina WHERE Id = ?`,
      [id],
      (err, results) => {
        if (err) {
          console.error();
          reject(err);
        } else {
          resolve(results);
        }
      }
    );
  });
}

async function getMaquinasActivas() {
  await conMySql();
  return new Promise((resolve, reject) => {
    db.query(
      `SELECT * from potencia_vial.Maquina WHERE Condicion = 'A'`,
      (err, results) => {
        if (err) {
          console.error();
          reject(err);
        } else {
          resolve(results);
        }
      }
    );
  });
}

async function getTipos() {
  await conMySql();
  return new Promise((resolve, reject) => {
    db.query(`SELECT * from potencia_vial.Tipo`, (err, results) => {
      if (err) {
        console.error();
        reject(err);
      } else {
        resolve(results);
      }
    });
  });
}

async function getMultimediaByMaquina() {
  await conMySql();
  return new Promise((resolve, reject) => {
    db.query(
      `SELECT * from potencia_vial.MaquinaMultimedia`,
      (err, results) => {
        if (err) {
          console.error();
          reject(err);
        } else {
          resolve(results);
        }
      }
    );
  });
}

async function getMultimedia() {
  await conMySql();
  return new Promise((resolve, reject) => {
    db.query(`SELECT * from potencia_vial.Multimedia`, (err, results) => {
      if (err) {
        console.error();
        reject(err);
      } else {
        resolve(results);
      }
    });
  });
}

module.exports = {
  getMaquinas,
  getMaquina,
  getMaquinasActivas,
  getTipos,
  getMultimediaByMaquina,
  getMultimedia,
};
