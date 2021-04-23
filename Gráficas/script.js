let url = "http://localhost:5000/api/chartData/";

async function getData(data) {

  let nivel1 = [ [], [], [] ]; // tiempo, # muertes y # comandos usados.
  let nivel2 = [ [], [], [] ];
  let nivel3 = [ [], [], [] ];

  for (let element of data) {
    let tiempo = element.tiempo.split(':');
    switch (element.nivel) {
      case "MUNDO_1/NIVEL_1":
        nivel1[0].push(parseInt(tiempo[0]) * 60 + parseInt(tiempo[1]));
        nivel1[1].push(element.muertes);
        nivel1[2].push(element.comandosUsados);
        break
      case "MUNDO_1/NIVEL_2":
        nivel2[0].push(parseInt(tiempo[0]) * 60 + parseInt(tiempo[1]));
        nivel2[1].push(element.muertes);
        nivel2[2].push(element.comandosUsados);
        break
      case "MUNDO_1/NIVEL_3":
        nivel3[0].push(parseInt(tiempo[0]) * 60 + parseInt(tiempo[1]));
        nivel3[1].push(element.muertes);
        nivel3[2].push(element.comandosUsados);
        break
    }
  }
  return [nivel1, nivel2, nivel3];
}

async function getChart(data) {
  console.log(data);
  const ctx = document.getElementById("myChart").getContext("2d");
  window.myBar = new Chart(ctx, {
    type: 'boxplot',
    data: {
      // define label tree
      labels: ['Nivel 1', 'Nivel 2', 'Nivel 3'],
      datasets: [{
        label: 'Tiempo',
        backgroundColor: 'rgba(255,0,0,0.5)',
        borderColor: 'red',
        borderWidth: 1,
        outlierColor: '#999999',
        padding: 10,
        itemRadius: 0,
        data: [
          data[0][0],
          data[1][0],
          data[2][0]
        ]
      }, {
        label: '# Muertes',
        backgroundColor: 'rgba(0,0,255,0.5)',
        borderColor: 'blue',
        borderWidth: 1,
        outlierColor: '#999999',
        padding: 10,
        itemRadius: 0,
        data: [
          data[0][1],
          data[1][1],
          data[2][1]
        ]
      },{
        label: '# Comandos Usados',
        backgroundColor: 'rgba(0,255,0,0.5)',
        borderColor: 'green',
        borderWidth: 1,
        outlierColor: '#999999',
        padding: 10,
        itemRadius: 0,
        data: [
          data[0][2],
          data[1][2],
          data[2][2]
        ]
      }],
    },
    options: {
      responsive: true,
      legend: {
        position: 'top',
      },
      title: {
        display: true,
        text: 'Chart.js Box Plot Chart'
      }
    }
  });
}

async function fetchData() {
  const response = await fetch(url);

  if (response.ok) { // if HTTP-status is 200-299
    // get the response body (the method explained below)
    const data = await response.json().then(getData);

    getChart(data);
  }

}

fetchData();
