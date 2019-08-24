import io.gatling.core.Predef._
import io.gatling.http.Predef_

class FirstScalaTests extends Simulation {

val jsonTestData = jsonFile("data/test-data.json").circullar

val rps_factor = Integer.getInteger("rpsFactor", 1).toInt
val time_factor  = Integer.getInteger("timeFactor", 1).toInt

val baseUrl = "http://master-perform.azurewebsites.net/api/master-perform/document"

val scenario = scenario("Create document and find similar document.")
    .feed(jsonTestData)
    .exec(http("Post document")
        .post(baseUrl)
        .body(StringBody(
            " 
                {
                  \"documentDetails\": ${DocumentDetails},
                  \"addresses\": ${Addresses},
                  \"findSimilar\": true   
                }
            "
            )
        .asJSON
        .header("Content-Type", "application/json-patch+json")
        .header("Accept", "application/json")
        .check(status.is(201))
    )

setUp(scenario.inject(s
      rampUsers(50 * rps_factor) over(5 * time_factor seconds),
      rampUsers(100 * rps_factor) over(10 * time_factor seconds),
    )).protocols(httpProtocol))
)