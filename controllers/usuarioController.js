var express = require("express");
const asyncHandler = require("express-async-handler");
const dataBase = require("../BD/mysql");
const jwt = require("jsonwebtoken");
const fs = require("fs");
const privateKeyPath = "private.key";

//let RSA_PRIVATE_KEY = "";

exports.validateUser = asyncHandler(async (req, res, next) => {
  const username = req.body.username;
  const password = req.body.password;

  const data = {
    username,
    password,
  };

  const response = {
    resultado: {
      statusCode: "",
      statusText: "",
    },
    user: {
      idUsuario: "",
      username: "",
      token: "",
    },
  };
  if (username && password) {
    try {
      const result = await dataBase.ejecutarConsulta(
        `SELECT * FROM potencia_vial.Usuario where Usuario = ? and Contraseña = ?`,
        [username, password]
      );
      console.log("result: " + result);
      if (result.length === 0) {
        response.resultado.statusCode = "404";
        response.resultado.statusText = "User Not Found";

        res.status(200).json(response);
      } else {
        console.log("Entro al else");
        response.resultado.statusCode = "200";
        response.resultado.statusText = "OK";

        response.user.idUsuario = result[0].Usuario;
        response.user.username = result[0].Contraseña;
        const idUser = result[0].Usuario.toString();
        await permisoLectura();
        const token = await generateJWT(idUser);
        response.user.token = token;

        res.cookie("SESSIONID", token, { httpOnly: true, secure: true });

        res.status(200).json(response);
      }
    } catch (err) {
      res.status(500).json({ error: "Internal Server Error: " + err });
    }
  } else {
    response.resultado.statusCode = "404";
    response.resultado.statusText = "User Not Found";

    res.status(200).json(response);
  }
});

const generateJWT = async (userId) => {
  try {
    const RSA_PRIVATE_KEY = fs.readFileSync("private.key");
    const jwtBearerToken = jwt.sign({}, RSA_PRIVATE_KEY, {
      algorithm: "RS256",
      expiresIn: 2000,
      subject: userId,
    });
    return jwtBearerToken;
  } catch (error) {
    console.error("Error generando el token JWT:", error);
    throw error;
  }
};

// Cambiar los permisos del archivo para darle permisos de lectura
async function permisoLectura() {
  fs.chmod(privateKeyPath, 0o400, (err) => {
    if (err) {
      console.error(
        `Error al cambiar los permisos del archivo ${privateKeyPath}: ${err.message}`
      );
    } else {
      console.log(
        `Permisos del archivo ${privateKeyPath} cambiados correctamente.`
      );
    }
  });
}
