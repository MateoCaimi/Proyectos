const mysql = require("mysql");
const config = require("../config/config");

const dbConfig = {
  host: config.host,
  user: config.user,
  password: config.password,
  database: config.database,
};

let db;

function conectar() {
  db = mysql.createConnection(dbConfig);

  return new Promise((resolve, reject) => {
    db.connect((err) => {
      if (err) {
        console.log("Érror al conectar: " + err);
        reject(err);
      } else {
        resolve();
      }
    });
  });
}

function desconectar() {
  return new Promise((resolve, reject) => {
    db.end((err) => {
      if (err) {
        console.log("Error al desconectar: " + err);
        reject(err);
      } else {
        resolve();
      }
    });
  });
}

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
  try {
    await conectar();
    return new Promise((resolve, reject) => {
      db.query("SELECT * from potencia_vial.Maquina", (err, results) => {
        if (err) {
          console.error(err);
          reject(err);
        } else {
          resolve(results);
        }
      });
    });
  } catch (err) {
    console.error(err);
  } finally {
    await desconectar();
  }
}

async function getMaquina(id) {
  try {
    await conectar();
    return new Promise((resolve, reject) => {
      db.query(
        `SELECT * from potencia_vial.Maquina WHERE Id = ?`,
        [id],
        (err, results) => {
          if (err) {
            console.error(err);
            reject(err);
          } else {
            resolve(results);
          }
        }
      );
    });
  } catch (err) {
    console.error(err);
  } finally {
    await desconectar();
  }
}

async function getMaquinasActivas() {
  try {
    await conectar();
    return new Promise((resolve, reject) => {
      db.query(
        `SELECT * from potencia_vial.Maquina WHERE Condicion = 'Nuevo'`,
        (err, results) => {
          if (err) {
            console.error(err);
            reject(err);
          } else {
            resolve(results);
          }
        }
      );
    });
  } catch (err) {
    console.error(err);
  } finally {
    await desconectar();
  }
}

async function getTipos() {
  try {
    await conectar();
    return new Promise((resolve, reject) => {
      db.query(`SELECT * from potencia_vial.Tipo`, (err, results) => {
        if (err) {
          console.error(err);
          reject(err);
        } else {
          resolve(results);
        }
      });
    });
  } catch (err) {
    console.error(err);
  } finally {
    await desconectar();
  }
}

async function getMultimediaByMaquina() {
  try {
    await conectar();
    return new Promise((resolve, reject) => {
      db.query(
        `SELECT * from potencia_vial.MaquinaMultimedia`,
        (err, results) => {
          if (err) {
            console.error(err);
            reject(err);
          } else {
            resolve(results);
          }
        }
      );
    });
  } catch (error) {
    console.error(error);
  } finally {
    await desconectar();
  }
}

async function getMultimedia() {
  try {
    await conectar();
    return new Promise((resolve, reject) => {
      db.query(`SELECT * from potencia_vial.Multimedia`, (err, results) => {
        desconectar().then(() => {
          if (err) {
            console.error(err);
            reject(err);
          } else {
            resolve(results);
          }
        });
      });
    });
  } catch (error) {
    console.error(error);
  } finally {
    await desconectar();
  }
}

async function ejecutarConsulta(query) {
  return new Promise((resolve, reject) => {
    db.query(query, (err, results) => {
      if (err) {
        reject(err);
      } else {
        resolve(results);
      }
    });
  });
}

async function insertarMultimedia(query, fileName, fileType, imageContent) {
  try {
    await conectar();
    return new Promise((resolve, reject) => {
      db.query(query, [fileType, fileName, imageContent], (err, results) => {
        if (err) {
          console.error(err);
          reject(err);
        } else {
          resolve(results);
        }
      });
    });
  } catch (error) {
    console.error(error);
  } finally {
    await desconectar();
  }
}

async function insertarMaquinaMultimedia(data) {
  try {
    await conectar();
    return new Promise((resolve, reject) => {
      db.query(
        `INSERT INTO potencia_vial.MaquinaMultimedia
      (MaquinaId, MultimediaId)
      VALUES(?,?);
      `,
        [data.MaquinaId, data.MultimediaId],
        (err, results) => {
          if (err) {
            console.error(err);
            reject(err);
          } else {
            resolve(results);
          }
        }
      );
    });
  } catch (error) {
    console.error(error);
  } finally {
    await desconectar();
  }
}

async function get_Multimedia(id) {
  try {
    await conectar();
    return new Promise((resolve, reject) => {
      db.query(
        `SELECT * FROM potencia_vial.Multimedia where Id = ?`,
        [id],
        (err, results) => {
          if (err) {
            console.error(err);
            reject(err);
          } else {
            resolve(results);
          }
        }
      );
    });
  } catch (error) {
    console.error(error);
  } finally {
    await desconectar();
  }
}

async function addMaquina(data) {
  try {
    await conectar();
    return new Promise((resolve, reject) => {
      db.query(
        `INSERT INTO potencia_vial.Maquina
      (Modelo, TipoId, Condicion, Altura, Largo, Ancho, CargaMaxima, descripcion, anio)
      VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?);
      `,
        [
          data.Modelo,
          data.TipoId,
          data.Condicion,
          data.Altura,
          data.Largo,
          data.Ancho,
          data.CargaMaxima,
          data.descripcion,
          data.anio,
        ],
        (err, results) => {
          if (err) {
            console.error(err);
            reject(err);
          } else {
            resolve(results);
          }
        }
      );
    });
  } catch (error) {
    console.error(error);
  } finally {
    await desconectar();
  }
}

module.exports = {
  getMaquinas,
  getMaquina,
  getMaquinasActivas,
  getTipos,
  getMultimediaByMaquina,
  getMultimedia,
  ejecutarConsulta,
  conectar,
  desconectar,
  insertarMultimedia,
  get_Multimedia,
  addMaquina,
  insertarMaquinaMultimedia,
};
