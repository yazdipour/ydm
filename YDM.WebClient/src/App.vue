<template>
  <v-app id="inspire" dark>
    <v-card flat>
      <v-toolbar color="red darken-3" extended flat>
        <v-card-text>
          <h2 class="text-xs-center display-1 font-weight-light">YDM</h2>
        </v-card-text>
      </v-toolbar>
      <v-layout row pb-2 style="background: #303030;">
        <v-flex xs10 offset-xs1 md6 offset-md3>
          <v-card class="card--flex-toolbar" style="margin-top: -64px;background: #212121;">
            <v-toolbar card prominent>
              <v-text-field color="white" label="Youtube Video URL / Search / PlayList" style="margin-top: 12px"></v-text-field>
              <v-btn round color="red"><v-icon>get_apps</v-icon>GET</v-btn>
              <v-btn icon color="red"><v-icon>search</v-icon></v-btn>
            </v-toolbar>
            <v-divider></v-divider>
            <!-- END TOOLBAR -->
            <p v-show="!videos.length" class="text-xs-center ma-4" >[[ Get/Search for a Video or Playlist ]]</p>
            <VideoView v-for="(item, index) in videos" :key="index" :video="videos[0]" :links="links[0]"/>
            <Footer/>
          </v-card>
        </v-flex>
      </v-layout>
    </v-card>
  </v-app>
</template>
<script>
  import Footer from './components/Footer'
  import VideoView from './components/VideoView'
  export default {
    data: ()=>({
      videos:[
        {
          Id: "tsMVj1VxSOU",
          Title: "Black Mirror: Bandersnatch Netflix Review",
          Duration: "787",
          Image: "http://ydm.herokuapp.com/getimage.php?i=tsMVj1VxSOU"
        }
      ],
      links: [
        [{
            url: "https://r2---sn-aigl6ner.c.doc-0-0-sj.sj.googleusercontent.com/videoplayback?dur=787.226&expire=1547117159&source=youtube&lmt=1546079100773385&id=o-AFMht8yLikR4uJFCm-oWl7aP_oWeBgvd8BpcnDiD70K-&ei=B842XIugHJGOgQfm2p1A&ms=au%2Crdu&mt=1547095386&fvip=2&mv=m&ipbits=0&ratebypass=yes&signature=DBBF493CBCEDE204832DC7C15B154A309F5B6B00.CA23A11D19B2CC0FAC4874C71C61291ADE5777A6&sparams=dur%2Cei%2Cid%2Cinitcwndbps%2Cip%2Cipbits%2Citag%2Clmt%2Cmime%2Cmm%2Cmn%2Cms%2Cmv%2Cpl%2Cratebypass%2Crequiressl%2Csource%2Cexpire&mn=sn-aigl6ner%2Csn-aigzrn7s&mime=video%2Fmp4&requiressl=yes&ip=54.155.140.118&key=yt6&pl=17&itag=22&initcwndbps=4670000&c=WEB&txp=5535432&mm=31%2C29&title=Black-Mirror-Bandersnatch-Netflix-Review",
            type: "video/mp4",
            quality: "hd720",
            tag: 0
        }]
      ]
    }),
    components: {
      Footer,VideoView
    },
    props: {
      source: String
    },
    methods: {},
  }
</script>