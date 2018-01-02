Vue.component('my-tree', {
		template: '#template_tree',
		props: ['treeData'],
		data: function () {
		  return {
		            baseId:1000,
		            defaultProps: {
		                children: 'children',
		                label: 'label',
		            }
		        }
		},
		computed: {
			data2:function(){
				return this.treeData;
			}
		},
		methods:{
            renderContent:function(createElement,{node:node}) {
                var self = this;
                return createElement('span', [
                    createElement('span', node.label)
                ]);
            },
            handleNodeClick:function(data){
            	if(!data['children']){
            		console.log(data);
            		this.$emit('selectcont',data.id);
            	}
            }
        }
	});