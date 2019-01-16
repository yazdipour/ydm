<template>
  <v-app id="inspire" dark>
    <v-card flat>
      <v-toolbar color="red darken-3" extended flat style="height:120px">
        <v-card-text>
          <h2 class="text-xs-center display-1 font-weight-light">YDM</h2>
        </v-card-text>
      </v-toolbar>
      <v-layout row pb-3 style="background: #303030;">
        <v-flex xs10 offset-xs1 md6 offset-md3>
          <v-card class="card--flex-toolbar" style="margin-top:-64px;background: #212121;">
            <v-toolbar card prominent>
              <v-text-field color="white" label="Youtube Video URL / Search / PlayList" style="margin-top: 12px" v-model="inputData"></v-text-field>
              <v-tooltip top>
                <v-btn :loading="loading" :disabled="loading" icon slot="activator" color="red" @click="getVideo"><v-icon>save_alt</v-icon></v-btn>
                <span>Get Video Links</span>
              </v-tooltip>
              <v-tooltip top>
                <v-btn :loading="loading2" :disabled="loading2" icon slot="activator" color="red" @click="getPlayList"><v-icon>list</v-icon></v-btn>
                <span>Get PlayList Videos</span>
              </v-tooltip>
              <v-tooltip top>
                <v-btn :loading="loading3" :disabled="loading3" icon slot="activator" color="red" @click="searchIt"><v-icon>search</v-icon></v-btn>
                <span>Search</span>
              </v-tooltip>
            </v-toolbar>
            <v-divider></v-divider>
            <!-- END TOOLBAR -->
            <p v-if="!videos" style="opacity:.2;" class="text-xs-center ma-4" >Get and Search Videos</p>
            <VideoView v-for="(item, index) in videos" :key="index" :video="item" :links="links[index]"/>
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
        // baseurl:"http://localhost:8000",
        baseurl:"https://ydm.herokuapp.com",
        inputData:"",
        videos:'',
        links:'',
        loading: false,
        loading2: false,
        loading3: false,
    }),
    components: {Footer,VideoView},
    props: {
        source: String
    },
    methods: {
        search(url){
            var _this=this;
            this.videos=[];
            this.links= [];
            this.$http.get(url).then((response) => {
                this.loading2=false;
                this.loading3=false;
                response.body.forEach(function (item) {
                    item.Image= _this.baseurl + "/getimage.php?i="+item.Id;
                    _this.links.push([]);
                });
                this.videos=response.body;
            });
        },
        getVideo(){
            if(this.inputData.length<2)return;
            this.loading=true;
            this.$http.get(this.baseurl+'/getvideo.php?i='+this.inputData).then((response) => {
                this.loading=false;
                this.videos=[response.body['info']];
                this.links=[response.body['links']];
            });
        },
        searchIt(){
            if(this.inputData.length<2)return;
            this.loading3 = true;
            this.search(this.baseurl+'/search/search.php?maxResults=20&q='+this.inputData);
        },
        getPlayList(){
            if(this.inputData.length<2)return;
            this.loading2 = true;
            this.search(this.baseurl+'/search/playlist.php?q='+this.inputData);
        }
    },
    created: function()
    {
        let url=window.location.href;
        if(url.includes('?v=')){
            this.inputData = url.substring(url.lastIndexOf('?v=')+3);
            this.getVideo();
        }
    }
}
</script>