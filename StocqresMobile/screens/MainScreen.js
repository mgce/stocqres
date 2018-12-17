import React, { Component } from "react";
import { StyleSheet, FlatList, View, TouchableOpacity } from "react-native";
import {
  Container,
  Header,
  Content,
  Left,
  Body,
  Title,
  Form,
  Item,
  Input,
  Label,
  Button,
  Text
} from "native-base";
import { connect } from "react-redux";
import { assignStockList } from "../ducks/stocks";
import {padding, colors, fonts} from '../styles/common'

const StockListItem = props => {
    const price = Math.round(props.price * 100) / 100
  return (
    <TouchableOpacity>
      <View style={listItemStyles.container}>
        <View style={listItemStyles.company}>
          <Text>{props.code}</Text>
          <Text style={listItemStyles.companyName}>{props.name}</Text>
        </View>
        <View style={listItemStyles.row}>
          <View style={listItemStyles.price}>
            <Text>{price} zl</Text>
          </View>
          <Button primary>
            <Text>Buy</Text>
          </Button>
        </View>
      </View>
    </TouchableOpacity>
  );
};

const listItemStyles = StyleSheet.create({
  container: {
    alignItems: "center",
    flex: 1,
    flexDirection: "row",
    justifyContent: "space-between",
    padding: padding.sm
  },
  company: {
    flexDirection: "column"
  },
  companyName: {
    fontSize: fonts.sm,
    color: colors.darkSecondary
  },
  row: {
    flexDirection: "row",
    alignItems: "center"
  },
  price: {
    fontSize: fonts.md,
    fontWeight: "bold",
    paddingLeft: padding.sm
  }
});

export class MainScreen extends Component {
  constructor(props) {
    super(props);
    this.state = {
        connected: false
    }
  }
  componentDidMount() {
    this.configureWebSockets();
  }
  configureWebSockets(){
    const ws = new WebSocket(
        "ws://webtask.future-processing.com:8068/ws/stocks"
      );
      ws.onopen = () => {
          this.setState({connected: true})
      }
      ws.onmessage = e => {
        console.log("Message: ", e.data);
        this.props.assignStockList(e.data);
      };
      ws.onclose = () => {
          this.setState({connected: false})
      }
  }
  _renderItem = ({ item }) => (
    <StockListItem
      id={item.id}
      code={item.Code}
      name={item.Name}
      unit={item.Unit}
      price={item.Price}
    />
  );
  render() {
    return (
      <Container>
        <Content>
          <FlatList data={this.props.stockList} renderItem={this._renderItem} />
        </Content>
      </Container>
    );
  }
}

const mapDispatchToProps = {
  assignStockList
};

const mapStateToProps = state => ({
  stockList: state.stocks.stockList
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(MainScreen);
