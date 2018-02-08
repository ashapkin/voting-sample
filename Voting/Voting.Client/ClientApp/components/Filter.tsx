import * as React from 'react';
import DatePicker  from 'react-datepicker';
import * as moment from 'moment';
import 'react-datepicker/dist/react-datepicker.css';

interface FilteringState {
	summary: string;
	date: moment.Moment | null;
	count: number;
	timeout: number;
	useServer : boolean;
}

class Filter extends React.Component<any, FilteringState> {

	constructor(props: any) {
		super(props);
		this.handleNumberSearch = this.handleNumberSearch.bind(this);
		this.handleTextSearch = this.handleTextSearch.bind(this);
		this.handleDateSearch = this.handleDateSearch.bind(this);
		this.handleServerLoadingFlag = this.handleServerLoadingFlag.bind(this);
		this.state = { summary: "", date: null, count: 0, timeout : 0, useServer : false };
	}

	handleTextSearch(event: any) {
		const self = this;
		const value = event.target.value;
		if (self.state.timeout) {
			clearTimeout(self.state.timeout);
		}
		this.setState({
			timeout: setTimeout(() => {
				const is_value_changed = self.state.summary != value;
				this.setState({ summary: value }, () => {
					if (is_value_changed) {
						self.fireActions();
					}
				});
			}, 500)
		});
	}

	handleNumberSearch(event: any) {
		const value = event.target.value;
		this.setState({ count: isFinite(value) ? value : -1, timeout : 0 },
			() => {
				this.fireActions();
			});
	}

	handleDateSearch(moment: moment.Moment) {
		this.setState({ date: moment },
			() => {
				this.fireActions();
			});
	}

	handleServerLoadingFlag(event: any) {
		const state = this.state;
		this.setState({ useServer: !state.useServer });
	}

	fireActions() {
		const state = this.state;
		if (!this.state.useServer) {
			console.log("Applying client filtering");
			return this.props.clientFilter((e: any) =>
				(e.summary.toLowerCase().search(state.summary) !== -1) &&
				(e.votesCount >= state.count) &&
				this.compareDates(state.date, e.dateTime));
		}
		console.log("Applying server filtering");
		let params = {
			date: this.state.date != null && this.state.date.isValid ? this.state.date.toJSON() : null,
			summary: this.state.summary,
			count: this.state.count
		};
		return this.props.remoteFilter(params);
	}

	compareDates(moment: any, date: string) {
		return (moment != null && moment.isValid) ? new Date(date) <= moment.toDate() : true;
	}

	render() {
		return (
			<div>
				<h3>Filters</h3>
				<div className="input-field">
					<input type="checkbox" id="serveLoading"
						onChange={this.handleServerLoadingFlag}
					    defaultChecked={this.state.useServer} />
					<label htmlFor="serveLoading">Use server filtering?</label>
				</div>
				<div className="input-field">
					<label>Search by summary</label>
					<input type="text" onKeyUp={this.handleTextSearch}/>
				</div>
				<div className={this.state.count < 0 ? "invalid-input-field" : "input-field"}>
					<label>Search by votes count (>=)</label>
					<input type="text" onKeyUp={this.handleNumberSearch} />
				</div>
				<div className="input-field">
					<label className="float-left">Search by Date</label>
					<div className="float-left">
						<DatePicker selected={this.state.date} onChange={this.handleDateSearch} />
					</div>
				</div>
			</div>
		);
	}
}

export default Filter;