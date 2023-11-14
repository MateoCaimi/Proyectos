const mysql = require("mysql");
const config = require("../config/config");

const dbConfig = {
  host: config.host,
  user: config.user,
  password: config.password,
  database: config.database,
};

let pool;

// Función para comprobar si la conexión está activa
async function verificarConexion(connection) {
  return connection && connection.state === "authenticated";
}

async function conectarPool() {
  return new Promise((resolve, reject) => {
    if (pool) {
      pool.getConnection(async (error, connection) => {
        if (error) {
          console.error("Error al obtener la conexión del pool:", error);
          reject(error);
        } else {
          if (!verificarConexion(connection)) {
            console.log("La conexión no está activa. Intentando reconectar...");
            connection.release(); // Libera la conexión actual
            connection = await pool.getConnection(); // Obtiene una nueva conexión del pool
          }
          resolve(connection); // Resuelve la promesa con la conexión activa
        }
      });
    } else {
      pool = mysql.createPool(dbConfig);
      pool.getConnection(async (error, connection) => {
        if (error) {
          console.error("Error al obtener la conexión del pool:", error);
          reject(error);
        } else {
          resolve(connection); // Resuelve la promesa con la conexión activa
        }
      });
    }
  });
}

async function getMaquinas() {
  let connection;

  try {
    connection = await conectarPool();
    if (connection) {
      const results = await new Promise((resolve, reject) => {
        connection.query(
          "SELECT * FROM potencia_vial.Maquina",
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

      connection.release(); // Libera la conexión después de usarla
      return results;
    } else {
      throw new Error("La conexión no se ha establecido correctamente.");
    }
  } catch (err) {
    console.error(err);
    throw err; // Re-lanza el error para que pueda ser manejado por código superior si es necesario
  }
}

async function getMaquina(id) {
  let connection;
  try {
    connection = await conectarPool();
    if (connection) {
      const results = await new Promise((resolve, reject) => {
        connection.query(
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
      connection.release(); // Libera la conexión después de usarla
      return results;
    } else {
      throw new Error("La conexión no se ha establecido correctamente.");
    }
  } catch (err) {
    console.error(err);
    throw err;
  }
}

async function ejecutarConsulta(query, params) {
  let connection;
  try {
    connection = await conectarPool();
    if (!connection) {
      throw new Error("La conexión no se ha establecido correctamente.");
    }
    const results = await new Promise((resolve, reject) => {
      connection.query(query, params, (err, results) => {
        if (err) {
          reject(err);
        } else {
          resolve(results);
        }
      });
    });

    return results;
  } catch (err) {
    console.error("Error en ejecutarConsulta:", err);
    throw new Error("Error al ejecutar la consulta.");
  } finally {
    if (connection) {
      connection.release(); // Libera la conexión después de usarla
    }
  }
}

async function addMaquina(data) {
  let connection;
  try {
    connection = await conectarPool();
    if (connection) {
      const results = new Promise((resolve, reject) => {
        connection.query(
          `INSERT INTO potencia_vial.Maquina
      (Modelo, TipoId, Condicion, Altura, Largo, Ancho, CargaMaxima, descripcion, anio,marca)
      VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?);
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
            data.Marca,
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
      connection.release();
      return results;
    } else {
      throw new Error("La conexión no se ha establecido correctamente.");
    }
  } catch (error) {
    console.error(error);
  }
}

async function get_Multimedia(id) {
  let connection;
  try {
    connection = await conectarPool();
    if (connection) {
      const results = new Promise((resolve, reject) => {
        connection.query(
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
      connection.release();
      return results;
    } else {
      throw new Error("La conexión no se ha establecido correctamente.");
    }
  } catch (error) {
    console.error(error);
  }
}

async function insertarMultimedia(query, fileName, fileType, imageContent) {
  let connection;
  try {
    connection = await conectarPool();
    if (connection) {
      const results = new Promise((resolve, reject) => {
        connection.query(
          query,
          [fileType, fileName, imageContent],
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
      connection.release();
      return results;
    } else {
      throw new Error("La conexión no se ha establecido correctamente.");
    }
  } catch (error) {
    console.error(error);
  }
}

async function insertarMaquinaMultimedia(data) {
  let connection;
  try {
    connection = await conectarPool();
    if (connection) {
      const results = new Promise((resolve, reject) => {
        connection.query(
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
      connection.release();
      return results;
    } else {
      throw new Error("La conexión no se ha establecido correctamente.");
    }
  } catch (error) {
    console.error(error);
  }
}

async function getTipos() {
  let connection;
  try {
    connection = await conectarPool();
    if (connection) {
      const results = new Promise((resolve, reject) => {
        connection.query(`SELECT * from potencia_vial.Tipo`, (err, results) => {
          if (err) {
            console.error(err);
            reject(err);
          } else {
            resolve(results);
          }
        });
      });
      connection.release();
      return results;
    } else {
      throw new Error("La conexión no se ha establecido correctamente.");
    }
  } catch (err) {
    console.error(err);
  }
}

async function insertarMaquinaMultimedia(data) {
  let connection;
  try {
    connection = await conectarPool();
    if (connection) {
      const results = new Promise((resolve, reject) => {
        connection.query(
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
      connection.release();
      return results;
    } else {
      throw new Error("La conexión no se ha establecido correctamente.");
    }
  } catch (error) {
    console.error(error);
  }
}

module.exports = {
  getMaquinas,
  getMaquina,
  getTipos,
  ejecutarConsulta,
  insertarMultimedia,
  get_Multimedia,
  addMaquina,
  insertarMaquinaMultimedia,
};
